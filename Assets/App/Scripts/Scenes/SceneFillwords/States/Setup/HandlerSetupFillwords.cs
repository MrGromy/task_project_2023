using System;
using System.Threading.Tasks;
using System.Xml;
using App.Scripts.Infrastructure.GameCore.States.SetupState;
using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels.View.ViewGridLetters;
using App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel;

namespace App.Scripts.Scenes.SceneFillwords.States.Setup
{
    public class HandlerSetupFillwords : IHandlerSetupLevel
    {
        private readonly ContainerGrid _containerGrid;
        private readonly IProviderFillwordLevel _providerFillwordLevel;
        private readonly IServiceLevelSelection _serviceLevelSelection;
        private readonly ViewGridLetters _viewGridLetters;

        public HandlerSetupFillwords(IProviderFillwordLevel providerFillwordLevel,
            IServiceLevelSelection serviceLevelSelection,
            ViewGridLetters viewGridLetters, ContainerGrid containerGrid)
        {
            _providerFillwordLevel = providerFillwordLevel;
            _serviceLevelSelection = serviceLevelSelection;
            _viewGridLetters = viewGridLetters;
            _containerGrid = containerGrid;
        }

        private int prevlvl = -1;
        public Task Process()
        {

            var model = _providerFillwordLevel.LoadModel(_serviceLevelSelection.CurrentLevelIndex);
            if (prevlvl == -1 && model != null)
            {
                prevlvl = _serviceLevelSelection.CurrentLevelIndex;
            }
            if (model == null)
            {
                int startlvl = _serviceLevelSelection.CurrentLevelIndex;
                if (prevlvl < _serviceLevelSelection.CurrentLevelIndex)
                {
                    while (model == null)
                    {
                        model = SwipeLvl(1, startlvl);
                    }
                }
                else
                {
                    while (model == null)
                    {
                        model = SwipeLvl(-1, startlvl);
                    }
                };
            }
            _viewGridLetters.UpdateItems(model);
            _containerGrid.SetupGrid(model, _serviceLevelSelection.CurrentLevelIndex);
            return Task.CompletedTask;
        }
        public GridFillWords SwipeLvl(int dir, int startlvl)
        {
            _serviceLevelSelection.UpdateSelectedLevel(_serviceLevelSelection.CurrentLevelIndex + dir);
            if (startlvl == _serviceLevelSelection.CurrentLevelIndex) throw new Exception("No correct levels detected");
            prevlvl = _serviceLevelSelection.CurrentLevelIndex;
            return _providerFillwordLevel.LoadModel(_serviceLevelSelection.CurrentLevelIndex);
            
        }
    }
}