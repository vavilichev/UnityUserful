using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VavilichevGD.Gameplay.Dialogues {
	[Serializable]
	public class DialogueInfo : ScriptableObject {
		
		public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
		public List<DialogueNodeData> dialogueNodeData = new List<DialogueNodeData>();

		public void Clear() {
			this.nodeLinks.Clear();
			this.dialogueNodeData.Clear();
		}

		public override bool Equals(object other) {
			if (other.GetType() != this.GetType())
				return false;

			var info = (DialogueInfo) other;

			var currentLinksCount = this.nodeLinks.Count;
			if (info.nodeLinks.Count != currentLinksCount)
				return false;

			for (int i = 0; i < currentLinksCount; i++) {
				var curNodeLink = this.nodeLinks[i];
				
				if (info.nodeLinks.All(e => e.portName != curNodeLink.portName))
					return false;
				
				if (info.nodeLinks.All(e => e.baseNodeGUID != curNodeLink.baseNodeGUID))
					return false;
				
				if (info.nodeLinks.All(e => e.targetNodeGUID != curNodeLink.targetNodeGUID))
					return false;
			}

			var curNodesCount = this.dialogueNodeData.Count;
			if (info.dialogueNodeData.Count != curNodesCount)
				return false;

			for (int i = 0; i < curNodesCount; i++) {
				var curNodeData = this.dialogueNodeData[i];

				if (info.dialogueNodeData.All(e => e.guid != curNodeData.guid))
					return false;
				
				if (info.dialogueNodeData.All(e => e.author != curNodeData.author))
					return false;
				
				if (info.dialogueNodeData.All(e => e.dialogueText != curNodeData.dialogueText))
					return false;
				
				if (info.dialogueNodeData.All(e => e.position != curNodeData.position))
					return false;
			}

			return true;
		}
	}
}