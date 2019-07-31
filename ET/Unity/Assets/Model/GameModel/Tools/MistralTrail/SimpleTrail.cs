/*
 ### Mistral Trail System ###
 Author: Jingping Yu
 RTX: joshuayu
 Created on: 2017/07/08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mistral.Effects.Trail
{
	/// <summary>
	/// Creates a simple and continuous trail. 
	/// </summary>
	public class SimpleTrail : TrailBase
	{
		#region Public Variables

		public float minVertexDistance = 0.1f;
		public int maxPointNumber = 50;

		#endregion

		#region Private Variables

		private Vector3 lastPosition;
		private float distanceMoved;

		#endregion

		#region MonoBehaviours

		protected override void Start()
		{
			base.Start();
			lastPosition = GetPosition();
		}

		protected override void Update()
		{
			if (isEmitting)
			{
                var pos= GetPosition();
                distanceMoved += Vector3.Distance(pos, lastPosition);
				
				if (distanceMoved != 0 && distanceMoved >= minVertexDistance)
				{
					AddPoint(new TrailPoint(), pos);
					distanceMoved = 0.0f;
				}
				lastPosition = pos;
			}
			base.Update();
		}

		#endregion

		#region Override Methods

		protected override void OnStartEmit()
		{
			lastPosition = GetPosition();
			distanceMoved = 0;
		}

		protected override void OnTranslate(Vector3 trans)
		{
			lastPosition += trans;
		}

		protected override int GetMaxPoints()
		{
			return maxPointNumber;
		}

        Vector3 GetPosition()
        {
            return TrailSpace.Local == this.parameter.trailSpace ? m_transform.localPosition : m_transform.position;
        }
		#endregion

	}

}