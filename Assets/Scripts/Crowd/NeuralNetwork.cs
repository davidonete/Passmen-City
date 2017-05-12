using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NN
{
    using Layer = List<Neuron>;

    public struct Connection
    {
        public double Weight;
        public double DeltaWeight;

        public Connection(double w) { Weight = w; DeltaWeight = 0.0; }
    }

    public class Neuron
    {
        int mIdx;
        double mGradient;
        double mOutputVal;
        public List<Connection> mOutputWeights = new List<Connection>();

        //Overall network learning rate [0.0...1.0]
        double mLearningRate = 0.15;
        //Multiplier of last weight change [0.0...n]
        double mMomentum = 0.5;

        public List<Connection> GetOutputWeights() { return mOutputWeights; }
        public void SetOutputWeights(List<Connection> weights) { mOutputWeights = weights; }

        double RandomWeight() { return UnityEngine.Random.Range(0.0f, 1.0f); }

        public Neuron(int numOutputs, int idx)
        {
            //Save the neuron index inside the layer for doing previous FeedForward.
            mIdx = idx;

            for (int connection = 0; connection < numOutputs; connection++)
            {
                //Create a connection for each output neuron and set a random weight
                mOutputWeights.Add(new Connection(RandomWeight()));
            }
        }

        public void FeedForward(ref Layer prevLayer)
        {
            double sum = 0.0;

            //Sum the previous layer's outputs (which are our inputs)
            //Include the bias neuron from the previous layer
            for (int neuronIdx = 0; neuronIdx < prevLayer.Count; neuronIdx++)
            {
                //F = Sum (neuron input value * neuron weight)
                sum += prevLayer[neuronIdx].GetOutputValue() *
                       prevLayer[neuronIdx].mOutputWeights[mIdx].Weight;
            }

            mOutputVal = TransferFunction(sum);
        }

        public void SetOutputValue(double value) { mOutputVal = value; }
        public double GetOutputValue() { return mOutputVal; }

        public void CalculateOutputGradients(double targetValue)
        {
            double delta = targetValue - mOutputVal;
            mGradient = delta * TransferFunctionDerivative(mOutputVal);
        }

        public void CalculateHiddenGradients(ref Layer nextLayer)
        {
            double dow = SumDOW(ref nextLayer);
            mGradient = dow * TransferFunctionDerivative(mOutputVal);
        }

        public void UpdateInputWeights(ref Layer prevLayer)
        {
            //Update the connection weights between this neuron and the previous layer
            for (int neuronIdx = 0; neuronIdx < prevLayer.Count; neuronIdx++)
            {
                Neuron neuron = prevLayer[neuronIdx];
                double oldDeltaWeight = neuron.mOutputWeights[mIdx].DeltaWeight;

                //The output value of the previous neurons magnified by the gradient and the learning rate
                //And the momentum will determine what percentage of the previous delta weight is maintained
                double newDeltaWeight = mLearningRate * neuron.GetOutputValue() * mGradient +
                                        mMomentum * oldDeltaWeight;

                var newWeight = neuron.mOutputWeights[mIdx];
                newWeight.DeltaWeight = newDeltaWeight;
                newWeight.Weight += newDeltaWeight;

                neuron.mOutputWeights[mIdx] = newWeight;
            }
        }

        double TransferFunction(double x)
        {
            //Hyperbolic tangential(tanh) = output range [-1.0...1.0]
            return Math.Tanh(x);
        }

        double TransferFunctionDerivative(double x)
        {
            //tanh derivative
            return 1.0 - x * x;
        }

        double SumDOW(ref Layer nextLayer)
        {
            double sum = 0.0;
            //Sum our contribution of the errors at the nodes we feed
            for (int neuronIdx = 0; neuronIdx < nextLayer.Count - 1; neuronIdx++)
                sum += mOutputWeights[neuronIdx].Weight * nextLayer[neuronIdx].mGradient;

            return sum;
        }
    }

    public class NeuralNetwork : MonoBehaviour
    {
        public int inputNeurones = 1;
        public int hiddenNeurones = 2;
        public int outputNeurones = 1;

        List<Layer> mLayers = new List<Layer>();
        double mError;
        double mRecentAverageError;
        double mRecentAverageSmoothingFactor;

        List<List<double>> mInputCache = new List<List<double>>();

        void InitializeNetwork(List<int> topology)
        {
            mRecentAverageError = 1.0;
            mRecentAverageSmoothingFactor = 1.0;

            //Create layers
            int numLayers = topology.Count;
            for (int layerIdx = 0; layerIdx < numLayers; layerIdx++)
            {
                mLayers.Add(new Layer());

                //if the layer is the last one there are no outputs
                //if not the number of outputs is the number of neurons of the next layer.
                int numOutputs; // = layerIdx == topology.size() - 1 ? 0 : topology[layerIdx + 1];
                if (layerIdx == numLayers - 1)
                    numOutputs = 0;
                else
                    numOutputs = topology[layerIdx + 1];

                //Fill the layer with neurons (<= we are addin an extra neuron with a constant value (Bias))
                int numNeurons = topology[layerIdx];
                for (int neuronIdx = 0; neuronIdx <= numNeurons; neuronIdx++)
                {
                    //Add the new neurons at the start of the array (LIFO)
                    mLayers[mLayers.Count - 1].Add(new Neuron(numOutputs, neuronIdx));
                }

                //Force the bias node's output value to 1.0
                mLayers[mLayers.Count - 1][mLayers[mLayers.Count - 1].Count - 1].SetOutputValue(-1.0);
            }
        }

        void FeedForward(List<double> inputVals)
        {
            //Set the output values of the first layer(input layer) as the input values they receive
            //Because the input layer does not modify this values at all.
            for (int i = 0; i < inputVals.Count; i++)
                mLayers[0][i].SetOutputValue(inputVals[i]);

            //Forward propagate
            //Go over each neuron of the network excluding the last neuron of each layer (the bias neuron)
            for (int layerIdx = 1; layerIdx < mLayers.Count; layerIdx++)
            {
                Layer prevLayer = mLayers[layerIdx - 1];
                for (int neuronIdx = 0; neuronIdx < mLayers[layerIdx].Count - 1; neuronIdx++)
                    mLayers[layerIdx][neuronIdx].FeedForward(ref prevLayer);
            }
        }

        void BackPropagate(List<double> targetVals)
        {
            //Calculate overall net error (Root mean square error(RMS) of output network errors)
            Layer outputLayer = mLayers[mLayers.Count - 1];
            mError = 0.0;

            //RMS = sqrt((sum((target value - actual value)^2)) / quantity of values - 1)
            for (int neuronIdx = 0; neuronIdx < outputLayer.Count - 1; neuronIdx++)
            {
                double delta = targetVals[neuronIdx] - outputLayer[neuronIdx].GetOutputValue();
                mError += delta * delta;
            }
            //Get average error squared (not including bias)
            mError /= outputLayer.Count - 1;
            //Get RMS
            mError = Math.Sqrt(mError);

            //Recent average measurement
            mRecentAverageError = (mRecentAverageError * mRecentAverageSmoothingFactor + mError) /
                                  (mRecentAverageSmoothingFactor + 1.0);

            //Calculate output layer gradients
            for (int neuronIdx = 0; neuronIdx < outputLayer.Count - 1; neuronIdx++)
                outputLayer[neuronIdx].CalculateOutputGradients(targetVals[neuronIdx]);

            //Calculate hidden layers gradients (from back to front)
            for (int layerIdx = mLayers.Count - 2; layerIdx > 0; layerIdx--)
            {
                Layer hiddenLayer = mLayers[layerIdx];
                Layer nextLayer = mLayers[layerIdx + 1];

                for (int neuronIdx = 0; neuronIdx < hiddenLayer.Count; neuronIdx++)
                    hiddenLayer[neuronIdx].CalculateHiddenGradients(ref nextLayer);
            }

            //Update connection weights for all layers except the input layer (from back to front)
            for (int layerIdx = mLayers.Count - 1; layerIdx > 0; layerIdx--)
            {
                Layer layer = mLayers[layerIdx];
                Layer prevLayer = mLayers[layerIdx - 1];

                for (int neuronIdx = 0; neuronIdx < layer.Count - 1; neuronIdx++)
                    layer[neuronIdx].UpdateInputWeights(ref prevLayer);
            }
        }

        public void SetConnectionWeights(List<double> w)
        {
            int connectionIdx = 0;
            //From the fist layer to the last hidden layer (except output layer)
            for (int layerIdx = 0; layerIdx < mLayers.Count - 1; layerIdx++)
            {
                Layer layer = mLayers[layerIdx];
                //From the first neuron of the layer to the last
                for (int neuronIdx = 0; neuronIdx < layer.Count; neuronIdx++)
                {
                    List<Connection> newConnections = new List<Connection>();
                    int numConnections = layer[neuronIdx].GetOutputWeights().Count;
                    for (int weigthIdx = 0; weigthIdx < numConnections; weigthIdx++)
                    {
                        newConnections.Add(new Connection(w[connectionIdx]));
                        connectionIdx++;
                    }
                    layer[neuronIdx].SetOutputWeights(newConnections);
                }
            }
        }

        public List<double> GetConnectionWeights()
        {
            List<double> w = new List<double>();

            //From the fist layer to the last hidden layer (except output layer)
            for (int layerIdx = 0; layerIdx < mLayers.Count - 1; layerIdx++)
            {
                Layer layer = mLayers[layerIdx];
                //From the first neuron of the layer to the last
                for (int neuronIdx = 0; neuronIdx < layer.Count; neuronIdx++)
                {
                    List<Connection> weights = layer[neuronIdx].GetOutputWeights();
                    for (int weigthIdx = 0; weigthIdx < weights.Count; weigthIdx++)
                        w.Add(weights[weigthIdx].Weight);
                }
            }

            return w;
        }

        void GetResults(List<double> resultVals)
        {
            resultVals.Clear();

            //Get the output values of the last layer (output layer) and save them on the result vals vector
            for (int neuronIdx = 0; neuronIdx < mLayers[mLayers.Count - 1].Count - 1; neuronIdx++)
                resultVals.Add(mLayers[mLayers.Count - 1][neuronIdx].GetOutputValue());
        }

        public void Init()
        {
            List<int> topology = new List<int>();
            topology.Add(inputNeurones);
            topology.Add(hiddenNeurones);
            topology.Add(outputNeurones);

            InitializeNetwork(topology);
        }

        List<double> GetTargetOutputs(List<double> inputs)
        {
            List<double> outputs = new List<double>();
            if(inputs[0] > 0.0)
                outputs.Add(0.0);
            else
                outputs.Add(1.0);
            return outputs;
        }

        public void TrainNeuralNetwork(List<List<double>> inputCache)
        {
            foreach (List<double> inputs in inputCache)
            {
                FeedForward(inputs);
                BackPropagate(GetTargetOutputs(inputs));
            }
        }

        public bool AskNeuralNetwork(CrossWalkBehaviour.CrossWalkStates state)
        {
            List<double> inputs = new List<double>();

            if (state == CrossWalkBehaviour.CrossWalkStates.kCrossWalkStates_RedLight)
                inputs.Add(0.0);
            else
                inputs.Add(1.0);

            //mInputCache.Add(inputs);

            FeedForward(inputs);

            List<double> outputs = new List<double>();
            GetResults(outputs);
            BackPropagate(GetTargetOutputs(inputs));

            //Debug.Log("NN input: " + inputs[0] + " output: " + outputs[0]);
            if (outputs[0] > 0.5)
                return true;

            return false;
        }

        public List<List<double>> GetInputCache() { return mInputCache; }
    }
}
