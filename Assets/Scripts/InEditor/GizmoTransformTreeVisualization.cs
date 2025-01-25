using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaMonks.MeshTools.Utility
{
	public class GizmoTransformTreeVisualization : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform _targetHolder;

		[Header("Settings")]
		[SerializeField] private bool _showAlways = true;
		[SerializeField] private int _depthLimit = -1;

		[Header("Visual Settings")]
		[SerializeField] private Color _color = Color.green;
		[SerializeField] private Shape _shape = Shape.Sphere;
		[SerializeField] private bool _wireframe = true;
		[Space]
		[SerializeField] private float _radius = 0.5f;
		[SerializeField] private bool _useChildTransform = false;

		private enum Shape
		{
			None,
			Cube,
			Sphere
		}


#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = _color;

			int counter = 0;

			var currentParent = _targetHolder == null ? transform : _targetHolder;
			DrawGizmosToChildren(currentParent, ref counter);
			DrawShape(currentParent);
		}

		private void DrawGizmosToChildren(Transform parent, ref int counter)
		{
			if (parent == null || (counter >= _depthLimit && _depthLimit >= 0))
			{
				return;
			}

			counter++;

			for (int i = 0; i < parent.childCount; i++)
			{
				var currentChild = parent.GetChild(i);
				DrawGizmosToChildren(currentChild, ref counter);

				DrawShape(currentChild);

				Gizmos.DrawLine(currentChild.transform.position, parent.transform.position);
			}
		}

		private void DrawShape(Transform t)
		{
			var originalMatrix = Gizmos.matrix;
			var pos = t.transform.position;

			if (_useChildTransform)
			{
				Gizmos.matrix = t.transform.localToWorldMatrix;
				pos = Vector3.zero;
			}

			switch (_shape)
			{
				case Shape.Sphere:

					var radius = _radius;// * (_useChildTransform ? currentChild.lossyScale.x : 1f);

					if (_wireframe) Gizmos.DrawWireSphere(pos, radius);
					else Gizmos.DrawSphere(pos, radius);

					break;

				case Shape.Cube:

					var size = _radius * 2 * Vector3.one;// (_useChildTransform ? currentChild.lossyScale : Vector3.one);

					if (_wireframe) Gizmos.DrawWireCube(pos, size);
					else Gizmos.DrawCube(pos, size);

					break;
			}

			Gizmos.matrix = originalMatrix;
		}

		private void OnDrawGizmos()
		{
			if (_showAlways)
			{
				OnDrawGizmosSelected();
			}
		}
#endif

	}
}