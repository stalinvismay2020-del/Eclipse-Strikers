#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "EclipseStrikersBall.generated.h"

UCLASS()
class ECLIPSESTRIKERS_API AEclipseStrikersBall : public AActor
{
    GENERATED_BODY()

public:
    AEclipseStrikersBall();

    UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category="Ball")
    TObjectPtr<class USphereComponent> Collision;

    UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category="Ball")
    TObjectPtr<class UStaticMeshComponent> BallMesh;

    UFUNCTION(BlueprintCallable, Category="Ball")
    void ResetBall(const FVector& NewLocation);
};