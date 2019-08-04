﻿using Hover.Core.Layouts.Rect;
using Hover.Core.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hover.Core.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcStack : HoverLayoutArcGroup, ILayoutableArc, ILayoutableRect {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcDegreesName = "ArcDegrees";
		public const string RectAnchorName = "RectAnchor";

		public enum ArrangementType {
			InnerToOuter,
			OuterToInner
		}

		[SerializeField]
		[DisableWhenControlled(DisplaySpecials=true)]
		[FormerlySerializedAs("Arrangement")]
		public ArrangementType _Arrangement = ArrangementType.InnerToOuter;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("OuterRadius")]
		public float _OuterRadius = 0.1f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0)]
		[FormerlySerializedAs("InnerRadius")]
		public float _InnerRadius = 0.04f;

		[SerializeField]
		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		[FormerlySerializedAs("ArcDegrees")]
		public float _ArcDegrees = 135;

		[SerializeField]
		[FormerlySerializedAs("Padding")]
		public HoverLayoutArcPaddingSettings _Padding;

		[SerializeField]
		[DisableWhenControlled(RangeMin=-180, RangeMax=180)]
		[FormerlySerializedAs("StartingDegree")]
		public float _StartingDegree = 0;

		[SerializeField]
		[DisableWhenControlled]
		[FormerlySerializedAs("RectAnchor")]
		public AnchorType _RectAnchor = AnchorType.MiddleCenter;

		private Vector2? vRectSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ArrangementType Arrangement {
			get => _Arrangement;
			set => this.UpdateValueWithTreeMessage(ref _Arrangement, value, "Arrangement");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float OuterRadius {
			get => _OuterRadius;
			set => this.UpdateValueWithTreeMessage(ref _OuterRadius, value, "OuterRadius");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float InnerRadius {
			get => _InnerRadius;
			set => this.UpdateValueWithTreeMessage(ref _InnerRadius, value, "InnerRadius");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float ArcDegrees {
			get => _ArcDegrees;
			set => this.UpdateValueWithTreeMessage(ref _ArcDegrees, value, "ArcDegrees");
		}

		/*--------------------------------------------------------------------------------------------*/
		public HoverLayoutArcPaddingSettings Padding {
			get => _Padding;
			set => this.UpdateValueWithTreeMessage(ref _Padding, value, "Padding");
		}

		/*--------------------------------------------------------------------------------------------*/
		public float StartingDegree {
			get => _StartingDegree;
			set => this.UpdateValueWithTreeMessage(ref _StartingDegree, value, "StartingDegree");
		}

		/*--------------------------------------------------------------------------------------------*/
		public AnchorType RectAnchor {
			get => _RectAnchor;
			set => this.UpdateValueWithTreeMessage(ref _RectAnchor, value, "RectAnchor");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();

			Padding.ClampValues(this);
			UpdateLayoutWithFixedSize();

			if ( vRectSize == null ) {
				Controllers.Set(RectAnchorName, this);
				RectAnchor = AnchorType.MiddleCenter;
			}

			vRectSize = null;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetArcLayout(float pOuterRadius, float pInnerRadius, 
												float pArcDegrees, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcDegreesName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcDegrees = pArcDegrees;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);

			OuterRadius = Mathf.Min(pSizeX, pSizeY)/2;
			vRectSize = new Vector2(pSizeX, pSizeY);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLayoutWithFixedSize() {
			int itemCount = vChildItems.Count;

			if ( itemCount == 0 ) {
				return;
			}

			bool isRev = (Arrangement == ArrangementType.OuterToInner);
			float relSumThickness = 0;
			float paddedOuterRadius = OuterRadius-Padding.OuterRadius;
			float paddedInnerRadius = InnerRadius+Padding.InnerRadius;
			float availDeg = ArcDegrees-Padding.StartDegree-Padding.EndDegree;
			float availThick = paddedOuterRadius-paddedInnerRadius-Padding.Between*(itemCount-1);
			float innerRadius = paddedInnerRadius;
			float paddedStartDeg = StartingDegree + (Padding.StartDegree-Padding.EndDegree)/2;

			Vector2 anchorPos = LayoutUtil.GetRelativeAnchorPosition(RectAnchor);
			anchorPos.x *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).x);
			anchorPos.y *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).y);

			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverLayoutArcGroupChild item = vChildItems[i];
				relSumThickness += item.RelativeThickness;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				HoverLayoutArcGroupChild item = vChildItems[childI];
				ILayoutableArc elem = item.Elem;
				float elemRelThick = availThick*item.RelativeThickness/relSumThickness;
				float elemRelArcDeg = availDeg*item.RelativeArcDegrees;
				float radiusOffset = elemRelThick*item.RelativeRadiusOffset;
				float elemStartDeg = paddedStartDeg + elemRelArcDeg*item.RelativeStartDegreeOffset;

				elem.SetArcLayout(
					innerRadius+elemRelThick+radiusOffset,
					innerRadius+radiusOffset,
					elemRelArcDeg,
					this
				);

				elem.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".x", this);
				elem.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".y", this);
				elem.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);

				Vector3 localPos = elem.transform.localPosition;
				localPos.x = anchorPos.x;
				localPos.y = anchorPos.y;

				elem.transform.localPosition = localPos;
				elem.transform.localRotation = Quaternion.AngleAxis(elemStartDeg, Vector3.back);

				innerRadius += elemRelThick+Padding.Between;
			}
		}

	}

}