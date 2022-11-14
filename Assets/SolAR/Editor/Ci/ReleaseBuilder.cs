/**
 * @copyright Copyright (c) 2022 B-com http://www.b-com.com/
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEditor;

namespace SolAR.ci
{
    class ReleaseBuilder
    {
        static void BuildAndroidApk()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { "Assets/SolAR/Demos/Scenes/NoviceVersion.unity" };
            buildPlayerOptions.locationPathName = "./SolARDemo.apk";
            buildPlayerOptions.target = BuildTarget.Android;
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
    }

} // namespace SolAR.ci