using HeartSim.classes.DataAndTypes;
using HeartSim.classes.NodeNS;
using HeartSim.classes.PathNS;
using System.Collections.Generic;

namespace HeartSim.classes.HeartNS
{

  public class Heart
  {
    private static Heart instance;
    private NodeTable m_nodeTable;
    private PathTable m_pathTable;

    private Heart(List<string> hNodeNames, List<List<int>> hNodeIntParameters,
                  List<Position> hNodePositions, List<string> hPathNames,
                  List<List<int>> hPathIntegerParameters, List<List<double>> hPathFloatParameters)
    {
      m_pathTable = new PathTable(hPathNames, hPathIntegerParameters, hPathFloatParameters);
      m_nodeTable = new NodeTable(hNodeNames, hNodeIntParameters, hNodePositions, m_pathTable);
    }

    public static Heart GetInstance()
    {
      if (instance == null)
      {
        // Initialize Data class or provide the required parameters
        instance = new Heart(Data.NodeNames, Data.NodeIntParameters, Data.NodePositions, Data.PathNames, Data.PathIntParameters, Data.PathDoubleParameters);
      }
      return instance;
    }

    public void HeartAutomaton()
    {
      const int ERP = 0; // Example value, replace with actual enum value
      const int Idle = 0; // Example value, replace with actual enum value

      NodeTable tempNode = m_nodeTable;
      PathTable tempPath = m_pathTable;
      PathTable pathTable = m_pathTable;
      NodeTable nodeTable = m_nodeTable;
      PathTable tempPathNode = m_pathTable;
      List<bool> tempAct = new List<bool>();

      for (int i = 0; i < m_nodeTable.node_table.Count; i++)
      {
        Node tempNodeElem = tempNode.node_table[i];
        tempNodeElem.SetIndexofPathActivateTheNode(-1);
        tempNodeElem.NodeAutomaton(tempPathNode);
        tempAct.Add(tempNodeElem.GetParameters().Activation);
      }

      for (int i = 0; i < m_pathTable.path_table.Count; i++)
      {
        (bool, bool) nodeActs = tempPath.path_table[i].PathAutomaton(nodeTable);
        bool nodeAct1 = nodeActs.Item1;
        bool nodeAct2 = nodeActs.Item2;
        Path path = pathTable.path_table[i];
        int entryIndex = path.GetParameters().EntryNodeIndex;
        if (nodeTable.node_table[entryIndex].GetParameters().NodeStateIndex != ERP)
        {
          tempAct[entryIndex] = tempAct[entryIndex] || nodeAct1;
          if (nodeAct1)
          {
            tempNode.node_table[entryIndex].SetIndexofPathActivateTheNode(i);
          }
        }
        else
        {
          tempAct[entryIndex] = false;
          nodeTable.node_table[entryIndex].SetTERP_Current(nodeTable.node_table[entryIndex].GetParameters().TERPDefault);
        }
        int exitIndex = path.GetParameters().ExitNodeIndex;
        if (nodeTable.node_table[exitIndex].GetParameters().NodeStateIndex != ERP)
        {
          tempAct[exitIndex] = tempAct[exitIndex] || nodeAct2;
          if (nodeAct2)
          {
            tempNode.node_table[exitIndex].SetIndexofPathActivateTheNode(i);
          }
        }
        else
        {
          tempAct[exitIndex] = false;
          nodeTable.node_table[exitIndex].SetTERP_Current(nodeTable.node_table[exitIndex].GetParameters().TERPDefault);
        }
      }

      for (int i = 0; i < m_nodeTable.node_table.Count; i++)
      {
        tempNode.node_table[i].SetActivation(tempAct[i]);
      }
      m_nodeTable = tempNode;

      for (int i = 0; i < tempPath.path_table.Count; i++)
      {
        if (tempPathNode.path_table[i].GetParameters().ForwardTimerDefault != tempPath.path_table[i].GetParameters().ForwardTimerDefault)
        {
          tempPath.path_table[i].SetForwardTimerCurrent(tempPathNode.path_table[i].GetParameters().ForwardTimerDefault);
          if (tempPathNode.path_table[i].GetParameters().PathStateIndex == Idle)
          {
            tempPath.path_table[i].SetForwardTimerCurrent(tempPath.path_table[i].GetParameters().ForwardTimerDefault);
          }
        }
        if (tempPath.path_table[i].GetParameters().BackwardTimerDefault != m_pathTable.path_table[i].GetParameters().BackwardTimerDefault)
        {
          m_pathTable.path_table[i].SetForwardTimerCurrent(tempPath.path_table[i].GetParameters().BackwardTimerDefault);
          if (tempPath.path_table[i].GetParameters().PathStateIndex == Idle)
          {
            m_pathTable.path_table[i].SetForwardTimerCurrent(m_pathTable.path_table[i].GetParameters().BackwardTimerDefault);
          }
        }
      }

      m_pathTable = tempPath;
    }

    public NodeTable GetNodeTable() => m_nodeTable;
    public PathTable GetPathTable() => m_pathTable;
  }
}
