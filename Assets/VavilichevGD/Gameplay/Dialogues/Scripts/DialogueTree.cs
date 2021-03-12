using System.Collections.Generic;
using System.Linq;

namespace VavilichevGD.Gameplay.Dialogues {
	public class DialogueTree {
		
		private Dictionary<string, DialogueNode> nodesMap;

		public DialogueTree(DialogueInfo info) {

			this.nodesMap = new Dictionary<string, DialogueNode>();

			foreach (var nodeData in info.dialogueNodeData) {
				var allOptionLinks = info.nodeLinks.Where(link => link.baseNodeGUID == nodeData.guid);
				var createdOptions = new List<DialogueOption>();
				foreach (var linkData in allOptionLinks) {
					var createdOption = new DialogueOption {
						text = linkData.portName,
						targetNodeGuid = linkData.targetNodeGUID
					};
					createdOptions.Add(createdOption);
				}
				
				var createdNode = new DialogueNode {
					author = nodeData.author,
					guid = nodeData.guid,
					text = nodeData.dialogueText,
					options = createdOptions
				};

				this.nodesMap[nodeData.guid] = createdNode;
			}
		}

		public DialogueNode GetNode(string guid) {
			this.nodesMap.TryGetValue(guid, out var node);
			return node;
		}

		public override string ToString() {
			var text = "Dialogue tree content: \n";

			var nodes = this.nodesMap.Values;
			foreach (var node in nodes) {
				text += $"{node.author} || {node.text} || {node.guid}\n";
				foreach (var option in node.options) 
					text += $"Option: {option.text}\n";
			}

			return text;
		}
	}
}