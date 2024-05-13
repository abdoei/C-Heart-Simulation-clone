using System;
using System.Collections.Generic;



namespace HeartSim.PathNS
{

  



  public class Path
  {
    private PathParameters _pathParameters;

    public Path(string pathName, List<int> pathIntegerParameters, List<float> pathFloatParameters)
    {
      _pathParameters = new PathParameters
      {
        PathName = pathName,
        PathStateIndex = (PathStateIndex)pathIntegerParameters[0],
        EntryNodeIndex = pathIntegerParameters[1] - 1,
        ExitNodeIndex = pathIntegerParameters[2] - 1,
        AmplitudeFactor = pathIntegerParameters[3],
        ForwardSpeed = pathFloatParameters[0],
        BackwardSpeed = pathFloatParameters[1],
        ForwardTimerCurrent = pathFloatParameters[2],
        ForwardTimerDefault = pathFloatParameters[3],
        BackwardTimerCurrent = pathFloatParameters[4],
        BackwardTimerDefault = pathFloatParameters[5],
        PathLength = pathFloatParameters[6],
        PathSlope = pathFloatParameters[7]
      };
    }

    public (bool, bool) PathAutomaton(NodeTable nt)
    {
      bool tempNode1Activation = false;
      bool tempNode2Activation = false;

      int entryNodeIndex = _pathParameters.EntryNodeIndex;
      int exitNodeIndex = _pathParameters.ExitNodeIndex;

      Node entryNode = nt.NodeTable[entryNodeIndex];
      Node exitNode = nt.NodeTable[exitNodeIndex];

      switch (_pathParameters.PathStateIndex)
      {
        case PathStateIndex.Idle:
          if (entryNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndex.AntegradeConduction;
          else if (exitNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndex.Retrograde;
          break;
        case PathStateIndex.AntegradeConduction:
          if (exitNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndex.Double;
          else
          {
            if (_pathParameters.ForwardTimerCurrent == 0)
            {
              _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
              tempNode2Activation = true;
              _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            }
            else
            {
              _pathParameters.ForwardTimerCurrent--;
            }
          }
          break;
        case PathStateIndex.Retrograde:
          if (entryNode.Parameters.Activation)
            _pathParameters.PathStateIndex = PathStateIndex.Double;
          else
          {
            if (_pathParameters.BackwardTimerCurrent == 0)
            {
              _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
              tempNode1Activation = true;
              _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            }
            else
            {
              _pathParameters.BackwardTimerCurrent--;
            }
          }
          break;
        case PathStateIndex.Conflict:
          _pathParameters.PathStateIndex = PathStateIndex.Idle;
          break;
        case PathStateIndex.Double:
          if (_pathParameters.BackwardTimerCurrent == 0)
          {
            _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
            tempNode1Activation = true;
            _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            return (tempNode1Activation, tempNode2Activation);
          }
          if (_pathParameters.ForwardTimerCurrent == 0)
          {
            _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
            tempNode2Activation = true;
            _pathParameters.PathStateIndex = PathStateIndex.Conflict;
            return (tempNode1Activation, tempNode2Activation);
          }
          if (Math.Abs(1 - (_pathParameters.ForwardTimerCurrent / _pathParameters.ForwardTimerDefault) -
                      (_pathParameters.BackwardTimerCurrent / _pathParameters.BackwardTimerDefault)) <
              (0.9 / Math.Min(_pathParameters.ForwardTimerDefault, _pathParameters.BackwardTimerDefault)))
          {
            _pathParameters.BackwardTimerCurrent = _pathParameters.BackwardTimerDefault;
            _pathParameters.ForwardTimerCurrent = _pathParameters.ForwardTimerDefault;
            _pathParameters.PathStateIndex = PathStateIndex.Conflict;
          }
          else
          {
            _pathParameters.ForwardTimerCurrent--;
            _pathParameters.BackwardTimerCurrent--;
          }
          break;
      }

      return (tempNode1Activation, tempNode2Activation);
    }

    // Getter and Setter methods for PathParameters can be added here
    // Example:
    // public PathParameters GetParameters() { return _pathParameters; }
    // public void SetParameters(PathParameters parameters) { _pathParameters = parameters; }
    // Other setter methods can be similarly added
    public PathParameters GetParameters() { return _pathParameters; }
    public void SetParameters(PathParameters parameters) { _pathParameters = parameters; }
    // setters 
    public void SetPathName(string pathName) { _pathParameters.PathName = pathName; }
    public void SetPathStateIndex(PathStateIndexEnum pathStateIndex) { _pathParameters.PathStateIndex = pathStateIndex; }
  }
}
