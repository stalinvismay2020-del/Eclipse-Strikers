#include "EclipseStrikersPitch.h"
#include "Components/SceneComponent.h"
#include "Components/StaticMeshComponent.h"
#include "UObject/ConstructorHelpers.h"

AEclipseStrikersPitch::AEclipseStrikersPitch()
{
    PrimaryActorTick.bCanEverTick = false;
    SceneRoot = CreateDefaultSubobject<USceneComponent>(TEXT("SceneRoot"));
    SetRootComponent(SceneRoot);
}

void AEclipseStrikersPitch::OnConstruction(const FTransform& Transform)
{
    Super::OnConstruction(Transform);
    TArray<USceneComponent*> Children;
    SceneRoot->GetChildrenComponents(false, Children);
    for (USceneComponent* Child : Children)
    {
        if (Child) Child->DestroyComponent();
    }

    AddBlock(FVector(0,0,-18), FVector(PitchLength/100.0f, PitchWidth/100.0f, 0.25f), FLinearColor(0.025f,0.22f,0.075f,1), TEXT("PitchSurface"));
    const float LineZ = 0.5f;
    const FLinearColor White(0.92f,0.96f,0.92f,1);
    AddBlock(FVector(0,-PitchWidth/2,LineZ), FVector(PitchLength/100.0f,0.035f,0.025f), White, TEXT("TouchlineSouth"));
    AddBlock(FVector(0, PitchWidth/2,LineZ), FVector(PitchLength/100.0f,0.035f,0.025f), White, TEXT("TouchlineNorth"));
    AddBlock(FVector(-PitchLength/2,0,LineZ), FVector(0.035f,PitchWidth/100.0f,0.025f), White, TEXT("GoalLineWest"));
    AddBlock(FVector( PitchLength/2,0,LineZ), FVector(0.035f,PitchWidth/100.0f,0.025f), White, TEXT("GoalLineEast"));
    AddBlock(FVector(0,0,LineZ), FVector(0.035f,PitchWidth/100.0f,0.025f), White, TEXT("HalfwayLine"));
    AddBlock(FVector(-PitchLength*0.28f,0,LineZ), FVector(0.035f,PitchWidth*0.38f/100.0f,0.025f), White, TEXT("BoxWest"));
    AddBlock(FVector( PitchLength*0.28f,0,LineZ), FVector(0.035f,PitchWidth*0.38f/100.0f,0.025f), White, TEXT("BoxEast"));

    const float GoalX = PitchLength/2 + 55.0f;
    const float GoalY = 145.0f;
    const float GoalH = 115.0f;
    for (int Side : {-1,1})
    {
        const float X = Side * GoalX;
        AddBlock(FVector(X,0,GoalH/2), FVector(1.1f,0.08f,GoalH/100.0f), White, Side < 0 ? TEXT("GoalWestBack") : TEXT("GoalEastBack"));
        AddBlock(FVector(X,-GoalY,GoalH/100.0f), FVector(1.1f,0.08f,GoalH/100.0f), White, TEXT("GoalPost"));
        AddBlock(FVector(X, GoalY,GoalH/100.0f), FVector(1.1f,0.08f,GoalH/100.0f), White, TEXT("GoalPost"));
        AddBlock(FVector(X,0,GoalH), FVector(1.1f,(GoalY*2)/100.0f,0.08f), White, TEXT("GoalCrossbar"));
    }
}

void AEclipseStrikersPitch::AddBlock(const FVector& Location, const FVector& Scale, const FLinearColor& Color, const TCHAR* Name)
{
    UStaticMeshComponent* Part = NewObject<UStaticMeshComponent>(this, FName(Name));
    Part->SetupAttachment(SceneRoot);
    Part->SetRelativeLocation(Location);
    Part->SetRelativeScale3D(Scale);
    static ConstructorHelpers::FObjectFinder<UStaticMesh> CubeAsset(TEXT("/Engine/BasicShapes/Cube.Cube"));
    if (CubeAsset.Succeeded()) Part->SetStaticMesh(CubeAsset.Object);
    Part->SetCollisionEnabled(ECollisionEnabled::QueryAndPhysics);
    Part->RegisterComponent();
    UMaterialInstanceDynamic* Material = UMaterialInstanceDynamic::Create(Part->GetMaterial(0), this);
    if (Material)
    {
        Material->SetVectorParameterValue(TEXT("BaseColor"), Color);
        Material->SetVectorParameterValue(TEXT("Color"), Color);
        Part->SetMaterial(0, Material);
    }
}