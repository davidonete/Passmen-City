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
        public List<Connection> mOutputWeights;

        //Overall network learning rate [0.0...1.0]
        double mLearningRate = 0.15;
        //Multiplier of last weight change [0.0...n]
        double mMomentum = 0.5;

        double RandomWeight() { return UnityEngine.Random.Range(0.0f, 1.0f); }

        Neuron(uint numOutputs, int idx)
        {
            //Save the neuron index inside the layer for doing previous FeedForward.
            mIdx = idx;

            for (uint connection = 0; connection < numOutputs; connection++)
            {
                //Create a connection for each output neuron and set a random weight
                mOutputWeights.Add(new Connection(RandomWeight()));
            }
        }

        void FeedForward(ref Layer prevLayer)
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
        }

        void SetOutputValue(double value) { mOutputVal = value; }
        double GetOutputValue() { return mOutputVal; }

        void CalculateOutputGradients(double targetValue)
        {
            double delta = targetValue - mOutputVal;
            mGradient = delta * TransferFunctionDerivative(mOutputVal);
        }

        void CalculateHiddenGradients(ref Layer nextLayer)
        {
            double dow = SumDOW(ref nextLayer);
            mGradient = dow * TransferFunctionDerivative(mOutputVal);
        }

        void UpdateInputWeights(ref Layer prevLayer)
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
}

public class NeuralNetwork : MonoBehaviour {

    public float inputs = 1;
    public float hidden = 2;
    public float output = 1;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
