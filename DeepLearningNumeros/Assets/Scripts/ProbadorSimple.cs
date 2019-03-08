using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbadorSimple : MonoBehaviour {
	void Start () {
        // 0 0 0    => 0
        // 0 0 1    => 1
        // 0 1 0    => 1
        // 0 1 1    => 0
        // 1 0 0    => 1
        // 1 0 1    => 0
        // 1 1 0    => 0
        // 1 1 1    => 1

        NeuralNetwork net = new NeuralNetwork(new int[] { 3, 25, 25, 1 });

		float n = 10f;

        for (int i = 0; i < 5000; i++)
        {
            net.FeedForward(new float[] { 0, 0, 0 });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { 0, 0, n });
            net.BackProp(new float[] { 1 });

            net.FeedForward(new float[] { 0, n, 0 });
            net.BackProp(new float[] { 1 });

            net.FeedForward(new float[] { 0, n, n });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { n, 0, 0 });
            net.BackProp(new float[] { 1 });

            net.FeedForward(new float[] { n, 0, n });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { n, n, 0 });
            net.BackProp(new float[] { 0 });

            net.FeedForward(new float[] { n, n, n });
            net.BackProp(new float[] { 1 });
        }


        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 0, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, 0, n })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, n, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { 0, n, n })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { n, 0, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { n, 0, n })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { n, n, 0 })[0]);
        UnityEngine.Debug.Log(net.FeedForward(new float[] { n, n, n })[0]);

    }
}
