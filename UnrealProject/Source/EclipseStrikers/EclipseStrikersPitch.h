#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "EclipseStrikersPitch.generated.h"

UCLASS()
class ECLIPSESTRIKERS_API AEclipseStrikersPitch : public AActor
{
    GENERATED_BODY()

public:
    AEclipseStrikersPitch();
    virtual void OnConstruction(const FTransform& Transform) override;

private:
    UPROPERTY(VisibleAnywhere)
    TObjectPtr<class USceneComponent> SceneRoot;

    UPROPERTY(EditAnywhere, Category="Pitch")
    float PitchLength = 1800.0f;

    UPROPERTY(EditAnywhere, Category="Pitch")
    float PitchWidth = 1100.0f;

    void AddBlock(const FVector& Location, const FVector& Scale, const FLinearColor& Color, const TCHAR* Name);
};