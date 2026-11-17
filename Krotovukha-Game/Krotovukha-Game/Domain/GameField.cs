using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Krotovukha_Game.Domain
{
    internal enum CellKind
    {
        Mole,
        Cabbage
    }

    internal sealed class GameCell
    {
        public CellKind Kind { get; internal set; }
        public int CreationIndex { get; }

        public GameCell(CellKind kind, int creationIndex)
        {
            Kind = kind;
            CreationIndex = creationIndex;
        }
    }

    /// <summary>Field state and transitions, independent of Windows Forms.</summary>
    internal sealed class GameField
    {
        public const int ColumnCount = 10;
        public const int CellCount = 100;

        private readonly List<GameCell> _cells = new List<GameCell>(CellCount);
        private readonly List<GameCell> _moles = new List<GameCell>(CellCount);
        private readonly List<GameCell> _cabbages = new List<GameCell>(CellCount);
        private readonly List<GameCell> _initialMoles = new List<GameCell>(CellCount);
        private readonly List<GameCell> _initialCabbages = new List<GameCell>(CellCount);
        private readonly Random _random;

        public ReadOnlyCollection<GameCell> Cells { get; }
        public int MoleCount => _moles.Count;

        public GameField(Random random)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            Cells = _cells.AsReadOnly();
        }

        public void Load(IEnumerable<CellKind> cells)
        {
            Clear();
            foreach (CellKind kind in cells)
            {
                AddCell(kind);
            }
            RememberInitialState();
        }

        public void Generate(int moleCount, Random random)
        {
            Clear();
            for (int i = 0; i < CellCount; i++)
            {
                AddCell(i < moleCount ? CellKind.Mole : CellKind.Cabbage);
            }

            // Preserve both the original shuffle and the selection order of mole cells.
            for (int i = 0; i < _cells.Count; i++)
            {
                int other = random.Next(CellCount);
                GameCell cell = _cells[i];
                _cells[i] = _cells[other];
                _cells[other] = cell;
            }
            RememberInitialState();
        }

        public GameCell PlantCabbage()
        {
            GameCell cell = _moles[_random.Next(_moles.Count)];
            _moles.Remove(cell);
            _cabbages.Add(cell);
            cell.Kind = CellKind.Cabbage;
            return cell;
        }

        public int ChooseColumn() => _random.Next(ColumnCount);

        public IEnumerable<GameCell> ClearColumn(int column)
        {
            for (int index = column; index < CellCount; index += ColumnCount)
            {
                GameCell cell = _cells[index];
                if (!_cabbages.Remove(cell))
                {
                    _moles.Remove(cell);
                }
                _moles.Add(cell);
                cell.Kind = CellKind.Mole;
                yield return cell;
            }
        }

        public void RestoreInitialState()
        {
            _moles.Clear();
            _cabbages.Clear();
            _moles.AddRange(_initialMoles);
            _cabbages.AddRange(_initialCabbages);
            foreach (GameCell cell in _moles)
            {
                cell.Kind = CellKind.Mole;
            }
            foreach (GameCell cell in _cabbages)
            {
                cell.Kind = CellKind.Cabbage;
            }
        }

        private void AddCell(CellKind kind)
        {
            var cell = new GameCell(kind, _cells.Count);
            _cells.Add(cell);
            (kind == CellKind.Mole ? _moles : _cabbages).Add(cell);
        }

        private void RememberInitialState()
        {
            _initialMoles.AddRange(_moles);
            _initialCabbages.AddRange(_cabbages);
        }

        private void Clear()
        {
            _cells.Clear();
            _moles.Clear();
            _cabbages.Clear();
            _initialMoles.Clear();
            _initialCabbages.Clear();
        }
    }
}
