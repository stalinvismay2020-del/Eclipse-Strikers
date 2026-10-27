#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Character.h"
#include "EclipseStrikersCharacter.generated.h"

UCLASS()
class ECLIPSESTRIKERS_API AEclipseStrikersCharacter : public ACharacter
{
    GENERATED_BODY()

public:
    AEclipseStrikersCharacter();

protected:
    virtual void BeginPlay() override;

public:
    virtual void Tick(float DeltaSeconds) override;
    virtual void SetupPlayerInputComponent(UInputComponent* PlayerInputComponent) override;

    UFUNCTION(BlueprintCallable, Category="Football")
    void Shoot();

    UFUNCTION(BlueprintCallable, Category="Football")
    void UseAbility();

private:
    void MoveForward(float Value);
    void MoveRight(float Value);

    UPROPERTY(VisibleAnywhere, Category="Camera")
    TObjectPtr<class USpringArmComponent> CameraBoom;

    UPROPERTY(VisibleAnywhere, Category="Camera")
    TObjectPtr<class UCameraComponent> FollowCamera;

    UPROPERTY(EditAnywhere, Category="Football")
    float ShotForce = 1500.0f;

    UPROPERTY(EditAnywhere, Category="Football")
    float AbilityCooldown = 8.0f;

    float AbilityReadyAt = 0.0f;
};