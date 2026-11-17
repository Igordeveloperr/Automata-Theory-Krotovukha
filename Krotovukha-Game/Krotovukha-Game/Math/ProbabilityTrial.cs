using System;
using System.Collections.Generic;

namespace Krotovukha_Game.Math
{
    internal sealed class ProbabilityTrial
    {
        private const int PercentScale = 100;
        private readonly int _percent;
        private readonly List<bool> _outcomes = new List<bool>(PercentScale);

        public ProbabilityTrial(int percent)
        {
            _percent = percent;
        }

        public bool Randomize()
        {
            // Keep the original sampling and per-call random lifetime unchanged.
            AddOutcomes(true, _percent);
            AddOutcomes(false, PercentScale - _percent);
            return _outcomes[new Random().Next(_outcomes.Count)];
        }

        private void AddOutcomes(bool outcome, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _outcomes.Add(outcome);
            }
        }
    }
}
