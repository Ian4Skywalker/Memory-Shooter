/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using UnityEngine.EventSystems;

namespace Meta.XR.ImmersiveDebugger.UserInterface
{
    /// <summary>
    /// Override of <see cref="OVRInputModule"/> which handles the case if there are more than one BaseInputModule on the same game object,
    /// It'll force process this input module for Immersive Debugger.
    /// For more info about Immersive Debugger, check out the [official doc](https://developer.oculus.com/documentation/unity/immersivedebugger-overview)
    /// </summary>
    public class PanelInputModule : OVRInputModule
    {
        private void Update()
        {
            if (eventSystem.currentInputModule != this)
            {
                Process(); // make sure this input module is always processed for Immersive Debugger
            }
        }
    }
}

