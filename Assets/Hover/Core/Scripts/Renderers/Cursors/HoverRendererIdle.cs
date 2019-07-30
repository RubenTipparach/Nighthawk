using System;
using Hover.Core.Renderers.CanvasElements;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Renderers.Cursors {

	/*================================================================================================*/
	public class HoverRendererIdle : HoverRenderer {

		public const string CenterPositionName = "CenterPosition";
		public const string DistanceThresholdName = "DistanceThreshold";
		public const string TimerProgressName = "TimerProgress";
		public const string IsRaycastName = "IsRaycast";

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("Fill")]
		private HoverFillIdle _Fill;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("CenterOffset")]
		private Vector3 _CenterOffset;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("DistanceThreshold")]
		private float _DistanceThreshold;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=1)]
		[FormerlySerializedAs("TimerProgress")]
		private float _TimerProgress;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("IsRaycast")]
		private bool _IsRaycast;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("RaycastOffsetZ")]
		private float _RaycastOffsetZ = -0.001f;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public HoverFillIdle Fill {
			get => _Fill;
			set => this.UpdateValueWithTreeMessage(ref _Fill, value, "Fill");
		}

		/*--------------------------------------------------------------------------------------------*/
		public Vector3 CenterOffset {
			get => _CenterOffset;
			set => this.UpdateValueWithTreeMessage(ref _CenterOffset, value, "CenterOffset");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float DistanceThreshold {
			get => _DistanceThreshold;
			set => this.UpdateValueWithTreeMessage(ref _DistanceThreshold, value, "DistanceThreshold");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float TimerProgress {
			get => _TimerProgress;
			set => this.UpdateValueWithTreeMessage(ref _TimerProgress, value, "TimerProgress");
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool IsRaycast {
			get => _IsRaycast;
			set => this.UpdateValueWithTreeMessage(ref _IsRaycast, value, "IsRaycast");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float RaycastOffsetZ {
			get => _RaycastOffsetZ;
			set => this.UpdateValueWithTreeMessage(ref _RaycastOffsetZ, value, "RaycastOffsetZ");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildFillCount() {
			return 1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverFill GetChildFill(int pIndex) {
			switch ( pIndex ) {
				case 0:
					return Fill;
			}

			throw new ArgumentOutOfRangeException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override int GetChildRendererCount() {
			return 0;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverRenderer GetChildRenderer(int pIndex) {
			throw new ArgumentOutOfRangeException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverCanvas GetCanvas() {
			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override HoverCanvasDataUpdater GetCanvasDataUpdater() {
			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetCenterWorldPosition() {
			return transform.position;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Vector3 pFromWorldPosition) {
			return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldPosition);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Vector3 GetNearestWorldPosition(Ray pFromWorldRay, out RaycastResult pRaycast) {
			return GetComponent<HoverShape>().GetNearestWorldPosition(pFromWorldRay, out pRaycast);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdatePosition();
			UpdateIndicator();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdatePosition() {
			if ( !Application.isPlaying || !IsRaycast ) {
				return;
			}

			Controllers.Set(SettingsControllerMap.TransformLocalPosition+".z", this);

			Vector3 localPos = transform.localPosition;
			localPos.z = RaycastOffsetZ/transform.lossyScale.z;
			transform.localPosition = localPos;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateIndicator() {
			if ( !Application.isPlaying ) {
				return;
			}

			HoverIndicator idleInd = GetComponent<HoverIndicator>();
			idleInd.Controllers.Set(HoverIndicator.HighlightProgressName, this);
			idleInd.HighlightProgress = Mathf.Lerp(0.05f, 1, TimerProgress);
		}

	}

}
