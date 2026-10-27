using UnrealBuildTool;
using System.Collections.Generic;

public class EclipseStrikersTarget : TargetRules
{
    public EclipseStrikersTarget(TargetInfo Target) : base(Target)
    {
        Type = TargetType.Game;
        DefaultBuildSettings = BuildSettingsVersion.V4;
        IncludeOrderVersion = EngineIncludeOrderVersion.Unreal5_4;
        ExtraModuleNames.Add("EclipseStrikers");
    }
}