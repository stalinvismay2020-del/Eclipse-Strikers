#include "EclipseStrikersGameMode.h"
#include "EclipseStrikersCharacter.h"
#include "EclipseStrikersPitch.h"
#include "EclipseStrikersBall.h"
#include "Engine/World.h"
#include "TimerManager.h"

AEclipseStrikersGameMode::AEclipseStrikersGameMode()
{
    DefaultPawnClass = AEclipseStrikersCharacter::StaticClass();
    PitchClass = AEclipseStrikersPitch::StaticClass();
    BallClass = AEclipseStrikersBall::StaticClass();
}

void AEclipseStrikersGameMode::StartPlay()
{
    Super::StartPlay();
    if (!GetWorld()) return;

    if (PitchClass) GetWorld()->SpawnActor<AEclipseStrikersPitch>(PitchClass, FVector::ZeroVector, FRotator::ZeroRotator);
    if (BallClass) GetWorld()->SpawnActor<AEclipseStrikersBall>(BallClass, FVector(0,0,45), FRotator::ZeroRotator);

    GetWorldTimerManager().SetTimer(MatchClockHandle, this, &AEclipseStrikersGameMode::TickMatchClock, 1.0f, true);
}

void AEclipseStrikersGameMode::TickMatchClock()
{
    if (SecondsRemaining > 0) --SecondsRemaining;
    if (SecondsRemaining <= 0) GetWorldTimerManager().ClearTimer(MatchClockHandle);
}