using HeartSim.classes.DataAndTypes;
using HeartSim.classes.PathNS;
using System;
using System.Collections.Generic;

namespace HeartSim.classes.NodeNS
{

  public class Node
  {
    private NodeParameters _nodeParameters;

    // Constructor
    public Node(string nodeName, List<int> nodeIntegerParameters, List<PathTerminalPair> pathTerminalPairs, Position nodePos)
    {
      _nodeParameters = new NodeParameters
      {
        NodeName = nodeName,
        NodeStateIndex = (NodeStateIndexEnum)nodeIntegerParameters[0],
        TERPCurrent = nodeIntegerParameters[1],
        TERPDefault = nodeIntegerParameters[2],
        TRRPCurrent = nodeIntegerParameters[3],
        TRRPDefault = nodeIntegerParameters[4],
        TrestCurrent = nodeIntegerParameters[5],
        TrestDefault = nodeIntegerParameters[6],
        Activation = Convert.ToBoolean(nodeIntegerParameters[7]),
        TerpMin = nodeIntegerParameters[8],
        TerpMax = nodeIntegerParameters[9],
        IndexOfPathActivateTheNode = nodeIntegerParameters[10],
        AVness = (AVnessEnum)nodeIntegerParameters[11],
        ConnectedPaths = pathTerminalPairs,
        NodePos = nodePos,
      };
    }

    public void NodeAutomaton(PathTable PT)
    {
      Random rand = new Random();

      bool t_activation = false; // temporary_activation

      if (_nodeParameters.Activation) // if node is activated
      {
        int t_Terp_min = _nodeParameters.TerpMin;
        int t_Terp_max = _nodeParameters.TerpMax;

        switch (_nodeParameters.NodeStateIndex)
        {
          case NodeStateIndexEnum.Rest: // Rest
                                        // set ERP to longest
            _nodeParameters.TERPDefault = t_Terp_max;
            // the diff between MatLab and C# round() is that in C# round() rounds
            // to the nearest integer value "round half to even", while in MatLab it
            // rounds towards the nearest integer value "round half away from zero"
            _nodeParameters.TERPCurrent =
                (_nodeParameters.TERPDefault +
                 (int)Math.Round((rand.NextDouble() - 0.5) * 0 * _nodeParameters.TERPDefault));

            // reset path conduction speed
            // for the paths connected to the node
            foreach (var path in _nodeParameters.ConnectedPaths)
            {
              int path_idx = path.PathIdx;
              if (_nodeParameters.IndexOfPathActivateTheNode == path_idx)
                continue; // skip the path that activated the node
              PathParameters pathParameters = PT.path_table[path_idx].GetParameters();
              double original_forward_speed = pathParameters.ForwardSpeed;
              double original_backward_speed = pathParameters.BackwardSpeed;
              double original_path_length = pathParameters.PathLength;
              // if at entry, only affect antegrade conduction; exit for
              // retrograde conduction
              if (path.Terminal == PathTerminalEnum.Entry)
              {
                double tmp_forward_timer_default =
                    Math.Round((rand.NextDouble() - 0.5) * 0 *
                               original_path_length / original_forward_speed);
                PT.path_table[path_idx].SetForwardTimerDefault(tmp_forward_timer_default);
              }
              else
              {
                double tmp_backward_timer_default =
                    Math.Round((rand.NextDouble() - 0.5) * 0 *
                               original_path_length / original_backward_speed);
                PT.path_table[path_idx].SetBackwardTimerDefault(tmp_backward_timer_default);
              }
            }

            // Reset Trest
            _nodeParameters.TrestCurrent = (int)Math.Round(
                _nodeParameters.TrestDefault * (rand.NextDouble() - 0.5) * 0);
            // change state to ERP
            _nodeParameters.NodeStateIndex = NodeStateIndexEnum.ERP;

            break;
          case NodeStateIndexEnum.ERP: // ERP
                                   // set ERP to the lowest
            _nodeParameters.TERPDefault = t_Terp_min;

            // set conduction speed to the slowest
            foreach (var path in _nodeParameters.ConnectedPaths)
            {
              int path_idx = path.PathIdx;
              if (_nodeParameters.IndexOfPathActivateTheNode == path_idx)
                continue; // skip the path that activated the node
              PathParameters pathParameters = PT.path_table[path_idx].GetParameters();
              double original_forward_speed = pathParameters.ForwardSpeed;
              double original_backward_speed = pathParameters.BackwardSpeed;
              double original_path_length = pathParameters.PathLength;
              // if at entry, only affect antegrade conduction; exit for
              // retrograde conduction
              if (path.Terminal == PathTerminalEnum.Entry)
              {
                double tmp_forward_timer_default =
                    Math.Round((rand.NextDouble() - 0.5) * 0 *
                               original_path_length / original_forward_speed * ((int)_nodeParameters.AVness + 1));
                PT.path_table[path_idx].SetForwardTimerDefault(tmp_forward_timer_default);
              }
              else
              {
                double tmp_backward_timer_default =
                    Math.Round((rand.NextDouble() - 0.5) * 0 *
                               original_path_length / original_backward_speed * 3);
                PT.path_table[path_idx].SetBackwardTimerDefault(tmp_backward_timer_default);
              }
            }

            // reset TERP
            _nodeParameters.TERPCurrent = (int)Math.Round(
                (rand.NextDouble() - 0.5) * 0 * _nodeParameters.TERPDefault);
            break;
          case NodeStateIndexEnum.RRP: // RRP
                                   // calculate the ratio of early activation
            double ratio = _nodeParameters.TRRPCurrent / _nodeParameters.TRRPDefault;
            // calculate the ERP timer for the next round

            if (_nodeParameters.AVness == AVnessEnum.AV)
            {
              _nodeParameters.TERPDefault =
                  t_Terp_max +
                  (int)Math.Round((rand.NextDouble() - 0.5) * 0 *
                             (1 - (1 - ratio) * (1 - ratio) * (1 - ratio)) *
                             (t_Terp_min - t_Terp_max));
            }
            else
            {
              _nodeParameters.TERPDefault =
                  t_Terp_min + (int)Math.Round((rand.NextDouble() - 0.5) * 0 *
                                          (1 - (ratio * ratio * ratio)) *
                                          (t_Terp_max - t_Terp_min));
            }

            _nodeParameters.TERPCurrent = (int)Math.Round(
                (rand.NextDouble() - 0.5) * 0 * _nodeParameters.TERPDefault);

            // change the conduction speed of connecting path
            foreach (var path in _nodeParameters.ConnectedPaths)
            {
              int path_idx = path.PathIdx;
              if (_nodeParameters.IndexOfPathActivateTheNode == path_idx)
                continue; // skip the path that activated the node
              PathParameters pathParameters = PT.path_table[path_idx].GetParameters();
              double original_path_length = pathParameters.PathLength;
              double original_forward_speed = pathParameters.ForwardSpeed;
              double original_backward_speed = pathParameters.BackwardSpeed;
              if (_nodeParameters.AVness == AVnessEnum.AV)
              {
                if (path.Terminal == PathTerminalEnum.Entry)
                {
                  double tmp_forward_timer_default =
                      Math.Round((rand.NextDouble() - 0.5) * 0 *
                                 original_path_length / original_forward_speed * (1 + ratio * 3));
                  PT.path_table[path_idx].SetForwardTimerDefault(tmp_forward_timer_default);
                }
                else
                {
                  double tmp_backward_timer_default =
                      Math.Round((rand.NextDouble() - 0.5) * 0 *
                                 original_path_length / original_backward_speed * (1 + ratio * 3));
                  PT.path_table[path_idx].SetBackwardTimerDefault(tmp_backward_timer_default);
                }
              }
              else
              {
                if (path.Terminal == PathTerminalEnum.Entry)
                {
                  double tmp_forward_timer_default =
                      Math.Round((rand.NextDouble() - 0.5) * 0 *
                                 original_path_length / original_forward_speed * (1 + ratio * ratio * 3));
                  PT.path_table[path_idx].SetForwardTimerDefault(tmp_forward_timer_default);
                }
                else
                {
                  double tmp_backward_timer_default =
                      Math.Round((rand.NextDouble() - 0.5) * 0 *
                                 original_path_length / original_backward_speed * (1 + ratio * ratio * 3));
                  PT.path_table[path_idx].SetBackwardTimerDefault(tmp_backward_timer_default);
                }
              }
            }

            // reset TRRP
            _nodeParameters.TRRPCurrent = (int)Math.Round(
                (rand.NextDouble() - 0.5) * 0 * _nodeParameters.TRRPDefault);
            // change state to ERP
            _nodeParameters.NodeStateIndex = NodeStateIndexEnum.ERP;
            break;
        }
      }
      else // if node is not activated
      {
        switch (_nodeParameters.NodeStateIndex)
        {
          case NodeStateIndexEnum.Rest: // Rest
            if (_nodeParameters.TrestCurrent == 0) // self depolarize
            {
              // change state to ERP
              _nodeParameters.NodeStateIndex = NodeStateIndexEnum.ERP;
              // reset Trest timer
              _nodeParameters.TrestCurrent = (int)Math.Round(
                  (rand.NextDouble() - 0.5) * 0 * _nodeParameters.TrestDefault);
              // activate the node
              t_activation = true;
            }
            else // timer
            {
              _nodeParameters.TrestCurrent--;
            }
            break;
          case NodeStateIndexEnum.ERP: // ERP
            if (_nodeParameters.TERPCurrent == 0) // timer running out
            {
              // change state to RRP
              _nodeParameters.NodeStateIndex = NodeStateIndexEnum.RRP;
              // reset TERP timer
              _nodeParameters.TERPCurrent = (int)Math.Round((rand.NextDouble() - 0.5) * 0 *
                                                        _nodeParameters.TERPDefault);
            }
            else // timer
            {
              _nodeParameters.TERPCurrent--;
            }
            break;
          case NodeStateIndexEnum.RRP: // RRP
            if (_nodeParameters.TRRPCurrent == 0) // timer running out
            {
              // change state to rest
              _nodeParameters.NodeStateIndex = NodeStateIndexEnum.Rest;
              // reset TRRP timer
              _nodeParameters.TRRPCurrent = (int)Math.Round((rand.NextDouble() - 0.5) * 0 *
                                                        _nodeParameters.TRRPDefault);
            }
            else // timer
            {
              _nodeParameters.TRRPCurrent--;
            }
            break;
        }
      }
      _nodeParameters.Activation = t_activation;
    }


    // Setter 
    public void SetNodeParameters(NodeParameters parameters) => _nodeParameters = parameters;
    // Getter method for node parameters
    public NodeParameters GetParameters() => _nodeParameters;
    public void SetParameters(NodeParameters parameters) => _nodeParameters = parameters;
    public void SetNodeName(string name) => _nodeParameters.NodeName = name;
    public void SetNodeStateIndex(NodeStateIndexEnum stateIndex) => _nodeParameters.NodeStateIndex = stateIndex;
    public void SetTERP_Current(int tERP_Current) => _nodeParameters.TERPCurrent = tERP_Current;
    public void SetTERP_Default(int tERP_Default) => _nodeParameters.TERPDefault = tERP_Default;
    public void SetTRP_Current(int tRRP_Current) => _nodeParameters.TRRPCurrent = tRRP_Current;
    public void SetTRRP_Default(int tRRP_Default) => _nodeParameters.TRRPDefault = tRRP_Default;
    public void SetTrest_Current(int trest_Current) => _nodeParameters.TrestCurrent = trest_Current;
    public void SetTrest_Default(int trest_Default) => _nodeParameters.TrestDefault = trest_Default;
    public void SetActivation(bool activation) => _nodeParameters.Activation = activation;
    public void SetTerpMin(int terpMin) => _nodeParameters.TerpMin = terpMin;
    public void SetTerpMax(int terpMax) => _nodeParameters.TerpMax = terpMax;
    public void SetIndexofPathActivateTheNode(int indexofPathActivateTheNode) => _nodeParameters.IndexOfPathActivateTheNode = indexofPathActivateTheNode;
    public void SetAVness(AVnessEnum aVness) => _nodeParameters.AVness = aVness;
    public void SetConnectedPaths(List<PathTerminalPair> connectedPaths) => _nodeParameters.ConnectedPaths = connectedPaths;
    public void SetPosition(Position position) => _nodeParameters.NodePos = position;


    // Setter methods for node parameters (if needed)
  }

  public class NodeTable
  {
    public List<Node> node_table;

    public NodeTable(List<string> nodeNames, List<List<int>> nodeIntParameters, List<Position> nodePositions, PathTable pathTable)
    {
      if (nodeNames.Count != nodeIntParameters.Count || nodeNames.Count != nodePositions.Count)
        throw new ArgumentException("nodeNames, nodeIntParameters, and nodePositions must have the same length.");

      node_table = new List<Node>(nodeNames.Count);

      for (int i = 0; i < nodeNames.Count; i++)
      {
        List<PathTerminalPair> pathTerminalPairs = pathTable.PathTerminalPairsPerPointList[i];
        node_table.Add(new Node(nodeNames[i], nodeIntParameters[i], pathTerminalPairs, nodePositions[i]));
      }
    }
  }

}
