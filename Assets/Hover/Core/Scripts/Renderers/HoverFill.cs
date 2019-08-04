﻿using Hover.Core.Renderers.Shapes;
using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers {

	/*================================================================================================*/
	[ExecuteInEditMode]
	[RequireComponent(typeof(TreeUpdater))]
	[RequireComponent(typeof(HoverShape))]
	public abstract class HoverFill : TreeUpdateableBehavior, ISettingsController {

		public ISettingsControllerMap Controllers { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected HoverFill() {
			Controllers = new SettingsControllerMap();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract int GetChildMeshCount();

		/*--------------------------------------------------------------------------------------------*/
		public abstract HoverMesh GetChildMesh(int pIndex);

		/*--------------------------------------------------------------------------------------------*/
		public HoverShape GetShape() {
			return gameObject.GetComponent<HoverShape>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			Controllers.TryExpireControllers();
		}

	}

}