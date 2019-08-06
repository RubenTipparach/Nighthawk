using System.Collections.Generic;
using Hover.Core.Renderers.Shapes;
using Hover.Core.Renderers.Utils;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Items.Sliders {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(HoverFillSlider))]
	[RequireComponent(typeof(HoverShape))]
	public abstract class HoverFillSliderUpdater : TreeUpdateableBehavior, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverFillSliderUpdater() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			UpdateFillMeshes();
			UpdateTickMeshes();
			Controllers.TryExpireControllers();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateFillMeshes() {
			HoverFillSlider fillSlider = gameObject.GetComponent<HoverFillSlider>();

			if ( fillSlider.SegmentInfo == null ) {
				return;
			}

			List<SliderUtil.SegmentInfo> segInfoList = fillSlider.SegmentInfo.SegmentInfoList;
			int segCount = HoverFillSlider.SegmentCount;
			int segIndex = 0;
			float startPos = segInfoList[0].StartPosition;
			float endPos = segInfoList[segInfoList.Count-1].EndPosition;

			for ( int i = 0 ; i < segInfoList.Count ; i++ ) {
				SliderUtil.SegmentInfo segInfo = segInfoList[i];

				if ( segInfo.Type != SliderUtil.SegmentType.Track ) {
					continue;
				}

				UpdateUsedFillMesh(fillSlider.GetChildMesh(segIndex++), segInfo, startPos, endPos);
			}

			for ( int i = 0 ; i < segCount ; i++ ) {
				HoverMesh segMesh = fillSlider.GetChildMesh(i);

				if ( i >= segIndex ) {
					UpdateUnusedFillMesh(segMesh);
				}

				ActivateFillMesh(segMesh);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateUnusedFillMesh(HoverMesh pSegmentMesh);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateUsedFillMesh(HoverMesh pSegmentMesh,
			SliderUtil.SegmentInfo pSegmentInfo, float pStartPos, float pEndPos);

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void ActivateFillMesh(HoverMesh pSegmentMesh);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected virtual void UpdateTickMeshes() {
			HoverFillSlider fillSlider = gameObject.GetComponent<HoverFillSlider>();

			if ( fillSlider.SegmentInfo == null ) {
				return;
			}

			List<SliderUtil.SegmentInfo> tickInfoList = fillSlider.SegmentInfo.TickInfoList;

			for ( int i = 0 ; i < tickInfoList.Count ; i++ ) {
				UpdateTickMesh(fillSlider.Ticks[i], tickInfoList[i]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected abstract void UpdateTickMesh(HoverMesh pTickMesh, SliderUtil.SegmentInfo pTickInfo);

	}

}
