using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pintar : MonoBehaviour {

	public Camera camara;
	public float radio;
	public float smooth;

	void Update()
	{
		if(Input.GetMouseButton(0))
		{
			RaycastHit hit;
			Ray ray = camara.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				Vector3 pos = Input.mousePosition;
				Transform objectHit = hit.transform;
				//objectHit.GetComponent<Renderer>().material.color = Color.white;
				Collider[] hitColliders = Physics.OverlapSphere(hit.point, radio);

				for(int i = 0; i < hitColliders.Length; i++)
				{
					float distancia = (hit.point - hitColliders[i].transform.position).sqrMagnitude;
					float calculo = (distancia/radio)*180;
					Color32  col = new Color32((byte)(180-calculo-smooth), (byte)(180-calculo-smooth), (byte)(180-calculo-smooth), (byte)255);
					if(Input.GetAxis("Mouse X")!= 0 || Input.GetAxis("Mouse Y")!= 0)
						hitColliders[i].transform.GetComponent<Renderer>().material.color += col;
				}
			}
		}
	}
}
