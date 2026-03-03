using Ad;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] CatOrbitManager catOrbit;
    [SerializeField] UpgradeUI ui;

    [Header("Timing (seconds)")]
    [SerializeField] float phase1Time = 5f;
    [SerializeField] float phase2Time = 12f;
    [SerializeField] float phase3Time = 18f;

    [SerializeField] CutSequenceController cutSequence;

    private float _elapsed;
    private int _nextPhaseIndex;
    private bool _pausedForChoice;

    private void Start()
    {
        ui.Hide();
    }

    private void Update()
    {
        if (_pausedForChoice) return;

        _elapsed += Time.deltaTime;

        switch (_nextPhaseIndex)
        {
            case 0 when _elapsed >= phase1Time:
                _ShowPhase(UpgradePhase.AttackSpeed);
                break;
            case 1 when _elapsed >= phase2Time:
                _ShowPhase(UpgradePhase.Multishot);
                break;
            case 2 when _elapsed >= phase3Time:
                _ShowPhase(UpgradePhase.Cats);
                break;
        }
    }

    private void _ShowPhase(UpgradePhase phase)
    {
        _pausedForChoice = true;
        AdGameFlow.Instance.SetState(AdState.Upgrade);
        Time.timeScale = 0f;

        ui.Show(phase, _BuildOptions(phase), _OnPick);
    }

    private UpgradeOption[] _BuildOptions(UpgradePhase phase)
    {
        switch (phase)
        {
            case UpgradePhase.Cats:
                return new[]
                {
                    new UpgradeOption{ type=UpgradeType.AddCats, intValue=1,  Title="+1 Cats",  Desc="Summon 1 cats"  },
                    new UpgradeOption{ type=UpgradeType.AddCats, intValue=3,  Title="+5 Cats",  Desc="Summon 3 cats"  },
                    new UpgradeOption{ type=UpgradeType.AddCats, intValue=10, Title="+10 Cats", Desc="Summon 10 cats" },
                };

            case UpgradePhase.AttackSpeed:
                // 초당 공격 횟수 합산
                return new[]
                {
                    new UpgradeOption{ type=UpgradeType.AttackSpeedMul, floatValue=0.5f, Title="Attack Speed +0.5", Desc="+0.5 attacks/sec" },
                    new UpgradeOption{ type=UpgradeType.AttackSpeedMul, floatValue=1.0f, Title="Attack Speed +1",   Desc="+1 attack/sec"  },
                    new UpgradeOption{ type=UpgradeType.AttackSpeedMul, floatValue=2.0f, Title="Attack Speed +2",   Desc="+2 attacks/sec" },
                };

            case UpgradePhase.Multishot:
                return new[]
                {
                    new UpgradeOption{ type=UpgradeType.AddMultishot, intValue=1, Title="Multishot +1", Desc="+1 rockets per shot" },
                    new UpgradeOption{ type=UpgradeType.AddMultishot, intValue=3, Title="Multishot +3", Desc="+3 rockets per shot" },
                    new UpgradeOption{ type=UpgradeType.AddMultishot, intValue=5, Title="Multishot +5", Desc="+5 rockets per shot" },
                };
        }

        return System.Array.Empty<UpgradeOption>();
    }

    private void _OnPick(UpgradePhase phase, UpgradeOption picked)
    {
        _Apply(picked);
        ui.Hide();

        _nextPhaseIndex++;
        _pausedForChoice = false;
        Time.timeScale = 1f;

        if (phase == UpgradePhase.Cats)
        {
            cutSequence.PlayFinalCut();
            return;
        }

        AdGameFlow.Instance.SetState(AdState.Playing);
    }

    private void _Apply(UpgradeOption opt)
    {
        var stats = catOrbit.GetSharedStats();

        switch (opt.type)
        {
            case UpgradeType.AddCats:
                catOrbit.AddCats(opt.intValue);
                break;

            case UpgradeType.AttackSpeedMul:
                stats.AddAttackSpeed(opt.floatValue); // 합산
                break;

            case UpgradeType.AddMultishot:
                stats.AddProjectiles(opt.intValue);
                break;
        }
    }
}
