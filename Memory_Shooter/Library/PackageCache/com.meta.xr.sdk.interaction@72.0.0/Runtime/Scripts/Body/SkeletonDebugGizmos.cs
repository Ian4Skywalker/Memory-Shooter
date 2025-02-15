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

using UnityEngine;

namespace Oculus.Interaction
{
    /// <summary>
    /// Draws debug gizmos representing a body skeleton. Joint positions, bones, and joint orientation axes can be drawn, and the visuals sized and colorized as desired.
    /// There are two implementations of SkeletonDebugGizmos provided, <cref="BodyDebugGizmos" /> and <cref="BodyPoseDebugGizmos" />.
    /// </summary>
    public abstract class SkeletonDebugGizmos : MonoBehaviour
    {
        [System.Flags]
        public enum VisibilityFlags
        {
            Joints = 1 << 0,
            Axes = 1 << 1,
            Bones = 1 << 2,
        }

        /// <summary>
        /// Which components of the skeleton will be visualized.
        /// </summary>
        [Tooltip("Which components of the skeleton will be visualized.")]
        [SerializeField]
        private VisibilityFlags _visibility =
            VisibilityFlags.Axes | VisibilityFlags.Joints;

        /// <summary>
        /// The joint debug spheres will be drawn with this color.
        /// </summary>
        [Tooltip("The joint debug spheres will be drawn with this color.")]
        [SerializeField]
        private Color _jointColor = Color.white;

        /// <summary>
        /// The bone connecting lines will be drawn with this color.
        /// </summary>
        [Tooltip("The bone connecting lines will be drawn with this color.")]
        [SerializeField]
        private Color _boneColor = Color.gray;

        /// <summary>
        /// The radius of the joint spheres and the thickness of the bone and axis lines.
        /// </summary>
        [Tooltip("The radius of the joint spheres and the thickness " +
            "of the bone and axis lines.")]
        [SerializeField]
        private float _radius = 0.02f;

        public float Radius
        {
            get => _radius;
            set => _radius = value;
        }

        public VisibilityFlags Visibility
        {
            get => _visibility;
            set => _visibility = value;
        }

        public Color JointColor
        {
            get => _jointColor;
            set => _jointColor = value;
        }

        public Color BoneColor
        {
            get => _boneColor;
            set => _boneColor = value;
        }

        private float LineWidth => _radius / 2f;

        protected abstract bool TryGetWorldJointPose(int jointId, out Pose pose);

        protected abstract bool TryGetParentJointId(int jointId, out int parent);

        protected bool HasNegativeScale => transform.lossyScale.x < 0 ||
                                           transform.lossyScale.y < 0 ||
                                           transform.lossyScale.z < 0;

        protected void Draw(int joint, VisibilityFlags visibility)
        {
            if (TryGetWorldJointPose(joint, out Pose pose))
            {
                if (visibility.HasFlag(VisibilityFlags.Axes))
                {
                    DebugGizmos.LineWidth = LineWidth;
                    DebugGizmos.DrawAxis(pose, _radius);
                }
                if (visibility.HasFlag(VisibilityFlags.Joints))
                {
                    DebugGizmos.Color = _jointColor;
                    DebugGizmos.LineWidth = _radius;
                    DebugGizmos.DrawPoint(pose.position);
                }
                if (visibility.HasFlag(VisibilityFlags.Bones) &&
                    TryGetParentJointId(joint, out int parent) &&
                    TryGetWorldJointPose(parent, out Pose parentPose))
                {
                    DebugGizmos.Color = _boneColor;
                    DebugGizmos.LineWidth = LineWidth;
                    DebugGizmos.DrawLine(pose.position, parentPose.position);
                }
            }
        }
    }
}
