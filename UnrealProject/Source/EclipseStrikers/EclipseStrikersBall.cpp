#include "EclipseStrikersBall.h"
#include "Components/SphereComponent.h"
#include "Components/StaticMeshComponent.h"
#include "UObject/ConstructorHelpers.h"

AEclipseStrikersBall::AEclipseStrikersBall()
{
    PrimaryActorTick.bCanEverTick = false;
    Collision = CreateDefaultSubobject<USphereComponent>(TEXT("BallCollision"));
    SetRootComponent(Collision);
    Collision->InitSphereRadius(22.0f);
    Collision->SetCollisionProfileName(TEXT("PhysicsActor"));
    Collision->SetSimulatePhysics(true);
    Collision->SetEnableGravity(true);
    Collision->SetLinearDamping(0.22f);
    Collision->SetAngularDamping(0.45f);

    BallMesh = CreateDefaultSubobject<UStaticMeshComponent>(TEXT("BallMesh"));
    BallMesh->SetupAttachment(Collision);
    static ConstructorHelpers::FObjectFinder<UStaticMesh> SphereAsset(TEXT("/Engine/BasicShapes/Sphere.Sphere"));
    if (SphereAsset.Succeeded()) BallMesh->SetStaticMesh(SphereAsset.Object);
    BallMesh->SetRelativeScale3D(FVector(0.22f));
    BallMesh->SetCollisionEnabled(ECollisionEnabled::NoCollision);
    BallMesh->SetSimulatePhysics(false);
}

void AEclipseStrikersBall::ResetBall(const FVector& NewLocation)
{
    SetActorLocation(NewLocation, false, nullptr, ETeleportType::TeleportPhysics);
    Collision->SetPhysicsLinearVelocity(FVector::ZeroVector);
    Collision->SetPhysicsAngularVelocityInDegrees(FVector::ZeroVector);
}