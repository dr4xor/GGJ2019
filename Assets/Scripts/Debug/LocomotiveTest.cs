using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocomotiveTest : MonoBehaviour
{
	[SerializeField]
	private GameObject waggonTestPrefab;

	[SerializeField] private int waggonCount;

	private void Start()
	{
		Locomotive lockomotive = GetComponent<Locomotive>();

		for(int i = 0; i < waggonCount; i++)
		{
			lockomotive.AddWaggon(Instantiate(waggonTestPrefab.GetComponent<Waggon>()));
		}
	}
}

