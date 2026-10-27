#include "EclipseStrikersCharacter.h"
#include "Camera/CameraComponent.h"
#include "GameFramework/SpringArmComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "Components/InputComponent.h"
#include "Engine/World.h"
#include "EngineUtils.h"
#include "GameFramework/Actor.h"
#include "GameFramework/ProjectileMovementComponent.h"
#include "PhysicsEngine/BodyInstance.h"

AEclipseStrikersCharacter::AEclipseStrikersCharacter()
{
    PrimaryActorTick.bCanEverTick = true;

    CameraBoom = CreateDefaultSubobject<USpringArmComponent>(TEXT("CameraBoom"));
    CameraBoom->SetupAttachment(GetRootComponent());
    CameraBoom->TargetArmLength = 360.0f;
    CameraBoom->bUsePawnControlRotation = true;
    CameraBoom->SetRelativeRotation(FRotator(-12.0f, 0.0f, 0.0f));

    FollowCamera = CreateDefaultSubobject<UCameraComponent>(TEXT("FollowCamera"));
    FollowCamera->SetupAttachment(CameraBoom, USpringArmComponent::SocketName);
    FollowCamera->bUsePawnControlRotation = false;

    bUseControllerRotationYaw = false;
    GetCharacterMovement()->bOrientRotationToMovement = true;
    GetCharacterMovement()->RotationRate = FRotator(0.0f, 540.0f, 0.0f);
    GetCharacterMovement()->MaxWalkSpeed = 650.0f;
    GetCharacterMovement()->JumpZVelocity = 420.0f;
}

void AEclipseStrikersCharacter::BeginPlay()
{
    Super::BeginPlay();
}

void AEclipseStrikersCharacter::Tick(float DeltaSeconds)
{
    Super::Tick(DeltaSeconds);
}

void AEclipseStrikersCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
    Super::SetupPlayerInputComponent(PlayerInputComponent);
    PlayerInputComponent->BindAxis(TEXT("MoveForward"), this, &AEclipseStrikersCharacter::MoveForward);
    PlayerInputComponent->BindAxis(TEXT("MoveRight"), this, &AEclipseStrikersCharacter::MoveRight);
    PlayerInputComponent->BindAction(TEXT("Jump"), IE_Pressed, this, &ACharacter::Jump);
    PlayerInputComponent->BindAction(TEXT("Shoot"), IE_Pressed, this, &AEclipseStrikersCharacter::Shoot);
    PlayerInputComponent->BindAction(TEXT("Ability"), IE_Pressed, this, &AEclipseStrikersCharacter::UseAbility);
}

void AEclipseStrikersCharacter::MoveForward(float Value)
{
    if (Controller && FMath::Abs(Value) > KINDA_SMALL_NUMBER)
    {
        AddMovementInput(FRotationMatrix(FRotator(0.0f, Controller->GetControlRotation().Yaw, 0.0f)).GetUnitAxis(EAxis::X), Value);
    }
}

void AEclipseStrikersCharacter::MoveRight(float Value)
{
    if (Controller && FMath::Abs(Value) > KINDA_SMALL_NUMBER)
    {
        AddMovementInput(FRotationMatrix(FRotator(0.0f, Controller->GetControlRotation().Yaw, 0.0f)).GetUnitAxis(EAxis::Y), Value);
    }
}

void AEclipseStrikersCharacter::Shoot()
{
    if (!GetWorld()) return;

    const FVector Start = GetActorLocation() + GetActorForwardVector() * 115.0f + FVector(0.0f, 0.0f, 25.0f);
    const FVector End = Start + GetActorForwardVector() * 220.0f;
    FHitResult Hit;
    FCollisionQueryParams Params;
    Params.AddIgnoredActor(this);

    if (GetWorld()->SweepSingleByChannel(Hit, Start, End, FQuat::Identity, ECC_PhysicsBody,
        FCollisionShape::MakeSphere(48.0f), Params))
    {
        if (UPrimitiveComponent* Component = Hit.GetComponent())
        {
            if (Component->IsSimulatingPhysics())
            {
                Component->AddImpulse(GetActorForwardVector() * ShotForce + FVector(0.0f, 0.0f, 260.0f), NAME_None, true);
            }
        }
    }
}

void AEclipseStrikersCharacter::UseAbility()
{
    if (!GetWorld() || GetWorld()->GetTimeSeconds() < AbilityReadyAt) return;
    AbilityReadyAt = GetWorld()->GetTimeSeconds() + AbilityCooldown;
    GetCharacterMovement()->MaxWalkSpeed = 1050.0f;
    GetWorldTimerManager().SetTimerForNextTick([this]()
    {
        if (GetCharacterMovement()) GetCharacterMovement()->MaxWalkSpeed = 650.0f;
    });
}