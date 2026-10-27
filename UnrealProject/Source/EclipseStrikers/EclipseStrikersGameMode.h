#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "EclipseStrikersGameMode.generated.h"

UCLASS()
class ECLIPSESTRIKERS_API AEclipseStrikersGameMode : public AGameModeBase
{
    GENERATED_BODY()

public:
    AEclipseStrikersGameMode();
    virtual void StartPlay() override;

    UFUNCTION(BlueprintPure, Category="Match")
    int32 GetSecondsRemaining() const { return SecondsRemaining; }

protected:
    UPROPERTY(EditDefaultsOnly, Category="Match")
    TSubclassOf<class AEclipseStrikersPitch> PitchClass;

    UPROPERTY(EditDefaultsOnly, Category="Match")
    TSubclassOf<class AEclipseStrikersBall> BallClass;

private:
    void TickMatchClock();

    int32 SecondsRemaining = 300;
    FTimerHandle MatchClockHandle;
};