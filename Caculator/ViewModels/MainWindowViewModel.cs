using Caculator.Models;
using ReactiveUI;
using System.Reactive;
using System;

namespace Caculator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

            private readonly CalculatorModel _calculator;
            private string _display;
            private string _currentInput;
            private bool _isNewCalculation;
            private string _lastOperation;

            public string Display
            {
                get => _display;
                private set => this.RaiseAndSetIfChanged(ref _display, value);
            }

            public ReactiveCommand<string, Unit> NumberCommand { get; }
            public ReactiveCommand<string, Unit> OperatorCommand { get; }
            public ReactiveCommand<Unit, Unit> EqualsCommand { get; }
            public ReactiveCommand<Unit, Unit> ClearCommand { get; }
            public ReactiveCommand<Unit, Unit> DecimalCommand { get; }
            public ReactiveCommand<Unit, Unit> BackspaceCommand { get; }

            public MainWindowViewModel()
            {
                _calculator = new CalculatorModel();
                _display = "0";
                _currentInput = string.Empty;
                _isNewCalculation = true;

                NumberCommand = ReactiveCommand.Create<string>(OnNumberPressed);
                OperatorCommand = ReactiveCommand.Create<string>(OnOperatorPressed);
                EqualsCommand = ReactiveCommand.Create(OnEqualsPressed);
                ClearCommand = ReactiveCommand.Create(OnClearPressed);
                DecimalCommand = ReactiveCommand.Create(OnDecimalPressed);
                BackspaceCommand = ReactiveCommand.Create(OnBackspacePressed);
            }

            private void OnNumberPressed(string number)
            {
                if (_isNewCalculation)
                {
                    _currentInput = string.Empty;
                    _isNewCalculation = false;
                }

                if (_currentInput == "0" && number == "0")
                    return;

                if (_currentInput == "0")
                    _currentInput = number;
                else
                    _currentInput += number;

                Display = _currentInput;
            }

            private void OnOperatorPressed(string op)
            {
                if (string.IsNullOrEmpty(_currentInput) && _calculator.FirstNumber == 0)
                    return;

                if (!string.IsNullOrEmpty(_currentInput))
                {
                    if (decimal.TryParse(_currentInput, out decimal number))
                    {
                        if (_calculator.Operation == null)
                        {
                            _calculator.FirstNumber = number;
                        }
                        else
                        {
                            _calculator.SecondNumber = number;
                            try
                            {
                                _calculator.Result = _calculator.Calculate();
                                _calculator.FirstNumber = _calculator.Result;
                                Display = _calculator.Result.ToString();
                            }
                            catch (DivideByZeroException)
                            {
                                HandleError("Cannot divide by zero");
                                return;
                            }
                        }
                    }
                }

                _calculator.Operation = op;
                _currentInput = string.Empty;
                _lastOperation = op;
                _isNewCalculation = false;
            }

            private void OnEqualsPressed()
            {
                if (string.IsNullOrEmpty(_calculator.Operation))
                    return;

                if (decimal.TryParse(_currentInput, out decimal number))
                {
                    _calculator.SecondNumber = number;
                    try
                    {
                        _calculator.Result = _calculator.Calculate();
                        Display = _calculator.Result.ToString();
                        _calculator.FirstNumber = _calculator.Result;
                        _currentInput = _calculator.Result.ToString();
                        _calculator.Operation = null;
                        _isNewCalculation = true;
                    }
                    catch (DivideByZeroException)
                    {
                        HandleError("Cannot divide by zero");
                    }
                    catch (InvalidOperationException)
                    {
                        HandleError("Invalid operation");
                    }
                }
            }

            private void OnClearPressed()
            {
                _calculator.FirstNumber = 0;
                _calculator.SecondNumber = 0;
                _calculator.Operation = null;
                _calculator.Result = 0;
                _currentInput = string.Empty;
                Display = "0";
                _isNewCalculation = true;
                _lastOperation = null;
            }

            private void OnDecimalPressed()
            {
                if (_isNewCalculation)
                {
                    _currentInput = "0";
                    _isNewCalculation = false;
                }

                if (string.IsNullOrEmpty(_currentInput))
                    _currentInput = "0";

                if (!_currentInput.Contains("."))
                {
                    _currentInput += ".";
                    Display = _currentInput;
                }
            }

            private void OnBackspacePressed()
            {
                if (_currentInput.Length > 0)
                {
                    _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
                    Display = string.IsNullOrEmpty(_currentInput) ? "0" : _currentInput;
                }
            }

            private void HandleError(string message)
            {
                Display = message;
                _calculator.FirstNumber = 0;
                _calculator.SecondNumber = 0;
                _calculator.Operation = null;
                _currentInput = string.Empty;
                _isNewCalculation = true;
            }
        }
    }

 

