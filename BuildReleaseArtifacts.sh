#!/bin/bash
# Copyright (c) 2022 B-com http://www.b-com.com/
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

if [ "SOLAR_UNITY_EXEC_PATH" == "" ]
then
  echo "Set SOLAR_UNITY_EXEC_PATH to the version of Unity you want to use with this script"
  exit -1
fi

"$SOLAR_UNITY_EXEC_PATH" -quit -batchmode -projectPath . -executeMethod SolAR.ci.ReleaseBuilder.BuildAndroidApk
"$SOLAR_UNITY_EXEC_PATH" -quit -batchmode -projectPath . -executeMethod SolAR.ci.ReleaseBuilder.ExportPackage

