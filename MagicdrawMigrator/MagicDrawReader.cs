﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Xml;
using System.IO;
using System.IO.Compression;
using EAAddinFramework.Utilities;
using TSF_EA = TSF.UmlToolingFramework.Wrappers.EA;
using UML = TSF.UmlToolingFramework.UML;
using System.Diagnostics;


namespace MagicdrawMigrator
{
	/// <summary>
	/// Description of MagicDrawReader.
	/// </summary>
	public class MagicDrawReader
	{
		string mdzipPath {get;set;}
		protected TSF_EA.Model model {get;set;}
		public string outputName {get;private set;}
		Dictionary<string,string> _allClasses;
		List<MDAssociation> _allASMAAssociations;
		Dictionary<string,string> _allLinkedAssociationTables;
		Dictionary<string, MDDiagram> _allDiagrams;
		Dictionary<string, string> _allObjects;
		Dictionary<string, string> _allPartitions;
		Dictionary<string, string> _allDependencies;
		Dictionary<string,string> _allLifeLines;
		Dictionary<string, string> _allMatrixes;
		List<MDFragment> _allFragments;
		List<MDMessage> _allMessages;
		List<MDAttribute> _allAttributes;
		Dictionary<string,List<MDConstraint>> allConstraints;
		Dictionary<string,XmlDocument> _sourceFiles;
		Dictionary<string,XmlDocument> sourceFiles
		{
			get
			{
				if (_sourceFiles == null)
				{
					_sourceFiles = new Dictionary<string, XmlDocument>();
					readMDSourceFiles();
				}
				return _sourceFiles;
			}
		}
		public MagicDrawReader(string mdzipPath,TSF_EA.Model model)
		{
			this.mdzipPath = mdzipPath;
			this.outputName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
			this.model = model;
		}
		private void readMDSourceFiles()
		{
			//create a new temp directory
			var tempDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(),"TmpMDZip"));
			//loop the files in the mdzip directory
			foreach (var fileName in Directory.GetFiles(mdzipPath,"*.mdzip")) 
			{
				//unzip all the mdzip file to their own subdirectory
				//check if the directory already exists and if so delete it
				string targetDirectory = Path.Combine(tempDirectory.FullName,Path.GetFileName(fileName));
				if (Directory.Exists(targetDirectory))
				{
					Directory.Delete(targetDirectory,true);
				}
				ZipFile.ExtractToDirectory(fileName,targetDirectory);
			}
			//loop all created subdirectories and read com.nomagic.magicdraw.uml_model.model and com.nomagic.magicdraw.uml_model.shared_model files as xml files
			foreach (var subDirectory in tempDirectory.GetDirectories())
			{
				var modelFile = subDirectory.GetFiles("com.nomagic.magicdraw.uml_model.model").FirstOrDefault();
				if (modelFile != null
				    && ! string.IsNullOrEmpty(modelFile.FullName))
				{
					var xmlModel =new XmlDocument();
					xmlModel.Load(modelFile.FullName);
					//add the xml documents to the dictionary of source files
					this._sourceFiles.Add(subDirectory.Name + "_Model",xmlModel);
				}
				var sharedModelFile = subDirectory.GetFiles("com.nomagic.magicdraw.uml_model.shared_model").FirstOrDefault();
				if (sharedModelFile != null
				    && ! string.IsNullOrEmpty(sharedModelFile.FullName))
				{
					var xmlModel =new XmlDocument();
					xmlModel.Load(sharedModelFile.FullName);
					//add the xml documents to the dictionary of source files
					this._sourceFiles.Add(subDirectory.Name + "_SharedModel",xmlModel);
				}
			}
		}
		
		public List<MDAttribute> allAttributes
		{
			get
			{
				if (_allAttributes == null)
				{
					this.getAllAttributes();
				}
				return _allAttributes;
			}
		}
		
		
		public List<MDFragment> allFragments
		{
			get
			{
				if (_allFragments == null)
				{
					this.getAllFragments();
				}
				return _allFragments;
			}
		}
		public List<MDMessage> allMessages
		{
			get
			{
				if (_allMessages == null)
				{
					this.getAllMessages();
				}
				return _allMessages;
			}
		}
		public Dictionary<string, string> allLifeLines 
		{
			get 
			{
				if (_allLifeLines == null)
				{
					this.getAllLifeLines();
				}
				return _allLifeLines;
			}
		}
		
		public Dictionary<string, string> allMatrixes
		{
			get 
			{
				if (_allMatrixes == null)
				{
					this.getAllMatrixes();
				}
				return _allMatrixes;
			}
		}

		public Dictionary<string, MDDiagram> allDiagrams
		{
			get
			{
				if (_allDiagrams == null)
				{
					this.getAllDiagrams();
				}
				return _allDiagrams;
			}
		}
		public Dictionary<string, string> allObjects
		{
			get
			{
				if (_allObjects == null)
				{
					this.getAllObjects();
				}
				return _allObjects;
			}
		}
		public Dictionary<string, string> allPartitions
		{
			get
			{
				if (_allPartitions == null)
				{
					this.getAllPartitions();
				}
				return _allPartitions;
			}
		}
		public Dictionary<string, string> allDependencies {
			get {
				if (_allDependencies == null)
				{
					this.getAllDependencies();
				}
				
				return _allDependencies;
			}
		}
		public Dictionary<string,string> allClasses
		{
			get
			{
				if (_allClasses == null)
				{
					this.getAllClasses();
				}
				return _allClasses;
			}
		}
		public Dictionary<string,string> allLinkedAssociationTables
		{
			get
			{
				if (_allLinkedAssociationTables == null)
				{
					this.getAllAssociationTables();
				}
				return _allLinkedAssociationTables;
			}
		}
		public List<MDAssociation> allASMAAssociations
		{
			get
			{
				if (_allASMAAssociations == null)
				{
					this.getAllASMAAssociations();
				}
				return _allASMAAssociations;
			}
		}
		public List<MDConstraint> getContraints(string MDElementID)
		{
			//check if the constraints were already read
			if (allConstraints == null)
			{
				allConstraints = getAllConstraints();
			}
			//check if list of constraints exists for the given id
			if (allConstraints.ContainsKey(MDElementID))
			{
				return allConstraints[MDElementID];
			}
			else
			{
				//return empty list if not found
				return new List<MDConstraint>();
			}
			
		}

		void getAllFragments()
		{
			var foundFragments = new List<MDFragment>();
			foreach (var sourceFile in this.sourceFiles.Values)
			{
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				//start with all xml nodes with name packagedElement and xmi:type='uml:Interaction'
				foreach (XmlNode interactionNode in sourceFile.SelectNodes("//packagedElement[@xmi:type='uml:Interaction']",nsMgr))
				{
					
					XmlAttribute interactionIDAttribute = interactionNode.Attributes["xmi:id"];
					if (interactionIDAttribute != null 
					    && !string.IsNullOrEmpty(interactionIDAttribute.Value))
					{
						string ownerID = interactionIDAttribute.Value;
						//get all xml nodes of with name fragment that have as xmi:type='uml:CombinedFragment'
						foreach (XmlNode fragmentNode in interactionNode.SelectNodes(".//fragment[@xmi:type='uml:CombinedFragment']",nsMgr)) 
						{
							//get the fragment mdID
							XmlAttribute fragmentIDAttribute = fragmentNode.Attributes["xmi:id"];
							string fragmentID = fragmentIDAttribute != null ? fragmentIDAttribute.Value:string.Empty;
							//get the fragment type
							XmlAttribute fragmentTypeAttribute = fragmentNode.Attributes["interactionOperator"];
							string fragmentType = fragmentTypeAttribute != null ? fragmentTypeAttribute.Value:string.Empty;
							if (! string.IsNullOrEmpty(fragmentID))
							{
								//create new MDFragment
								var mdFragment = new MDFragment(ownerID,fragmentID,fragmentType);
								foreach (XmlNode operandNode in fragmentNode.SelectNodes("./operand",nsMgr)) 
								{
									XmlNode guardNode = operandNode.SelectSingleNode("./guard/specification",nsMgr);
									//get the guard text
									XmlAttribute guardValueAttribute = guardNode != null ? guardNode.Attributes["value"] : null;
									//if the guard is empty then the default is "else"
									string operandGuard = guardValueAttribute != null ? guardValueAttribute.Value:"else";
									if (! string.IsNullOrEmpty(operandGuard))
									{
										//add it to the fragment
										mdFragment.operandGuards.Add(operandGuard);
									}
								}
								//add the fragment to the list of found fragments
								foundFragments.Add(mdFragment);
							}
						} 
					}
				}
			}
			//set the fragments
			_allFragments = foundFragments;
		}
		
		void getAllASMAAssociations()
		{
			var foundAssociations = new List<MDAssociation>();
			foreach (var sourceFile in this.sourceFiles.Values)
			{
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				nsMgr.AddNamespace("Business_Document_Library", "http://www.magicdraw.com/schemas/Business_Document_Library.xmi");
				//get the associations that are missing by looking at the tags with Business_Document_Library:ASMA
				foreach (XmlNode asmaNode in sourceFile.SelectNodes("//*[local-name() = 'ASMA']",nsMgr)) 
				{
					MDAssociationEnd mdTargetEnd = null;
					MDAssociationEnd mdSourceEnd = null;
					// and getting the ID of their attributebase_Association
					XmlAttribute baseAssociationAttribute = asmaNode.Attributes["base_Association"];
					if (baseAssociationAttribute != null)
					{
						string associationID = baseAssociationAttribute.Value;
						// then get the association itself by looking at the nodes <packagedElement> with attribute  xmi:type='uml:Association' and xmi:id= the id of the association
						foreach (XmlNode associationNode in sourceFile.SelectNodes("//packagedElement[@xmi:id='"+associationID+"']",nsMgr))					                                                           
						{
							XmlNode targetEndNode= null;
							XmlNode ownedEndNode = associationNode.SelectSingleNode("./ownedEnd");
							if (ownedEndNode != null)
							{
								XmlAttribute typeIDAttribute = ownedEndNode.Attributes["type"];
								XmlAttribute endIDAttribute = ownedEndNode.Attributes["xmi:id"];
								if (typeIDAttribute != null
								   && endIDAttribute != null)
								{
									string typeID = typeIDAttribute.Value;
									string endID = endIDAttribute.Value;
									foreach (XmlNode memberNode in associationNode.SelectNodes("./memberEnd"))
									{
										XmlAttribute idRefAttribute = memberNode.Attributes["xmi:idref"];
										if (idRefAttribute != null
										    && !idRefAttribute.Value.Equals(endID))
										{
											targetEndNode = memberNode;
										}
										else
										{
											//create sourceEnd
											mdSourceEnd = new MDAssociationEnd();
											mdSourceEnd.aggregationKind = "shared";
											mdSourceEnd.endClassID = typeIDAttribute.Value;
										}
									}
									if (targetEndNode != null)
									{
										XmlAttribute idRefAttribute = targetEndNode.Attributes["xmi:idref"];
										if (idRefAttribute != null)
										{
											string attributeID = idRefAttribute.Value;
											//find the ownedAttribute node with this xmi:id
											XmlNode attributeNode = sourceFile.SelectSingleNode("//ownedAttribute[@xmi:id='"+attributeID+"']",nsMgr);
											if (attributeNode != null)
											{
												string name = string.Empty;
												string lowerBound = string.Empty;
												string upperBound = string.Empty;
												string targetTypeID = string.Empty;
												//get the name from the attribute name
												XmlAttribute nameAttribute = attributeNode.Attributes["name"];
												name = nameAttribute != null ? nameAttribute.Value: string.Empty;
												//get the targeTypeID from attribute href (after the # sign) in the subnode type
												XmlNode targetTypeNode = attributeNode.SelectSingleNode("./type");
												if (targetTypeNode != null)
												{
													XmlAttribute hrefAttribute = targetTypeNode.Attributes["href"];
													string fullHrefValue = hrefAttribute != null ? hrefAttribute.Value : string.Empty;
													//get the part after the # sign
													var splittedHref = fullHrefValue.Split('#');
													if (splittedHref.Count() == 2)
													{
														targetTypeID = splittedHref[1];
													}
												}
												//get the lowerBound
												XmlNode lowerBoundNode = attributeNode.SelectSingleNode(".//lowerValue");
												if (lowerBoundNode != null)
												{
													XmlAttribute valueAttribute = lowerBoundNode.Attributes["value"];
													lowerBound = valueAttribute != null ? valueAttribute.Value: "0";
												}
												//get the lowerBound
												XmlNode upperBoundNode = attributeNode.SelectSingleNode(".//upperValue");
												if (upperBoundNode != null)
												{
													XmlAttribute valueAttribute = upperBoundNode.Attributes["value"];
													upperBound = valueAttribute != null ? valueAttribute.Value: string.Empty;
												}
												//create the targetEnd
												if (! string.IsNullOrEmpty(targetTypeID))
												{
													mdTargetEnd = new MDAssociationEnd();
													mdTargetEnd.name = name;
													mdTargetEnd.lowerBound = lowerBound;
													mdTargetEnd.upperBound = upperBound;
													mdTargetEnd.endClassID = targetTypeID;
												}
											}
											
										}
									}
									//create the association
									if (mdSourceEnd != null 
									    && mdTargetEnd != null)
									{
										var newASMAAssocation = new MDAssociation(mdSourceEnd,mdTargetEnd);
										newASMAAssocation.stereotype = "ASMA";
										foundAssociations.Add( newASMAAssocation);
									}
								}
							}						
						}
					}
				} 
				
			}
			//set the collection to the found associations
			_allASMAAssociations = foundAssociations;
		}

		void getAllMessages()
		{
			var foundMessages = new List<MDMessage>();
			//first find all the Lifeline nodes
			foreach (var sourceFile in this.sourceFiles.Values)
			{
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				foreach (XmlNode messageNode in sourceFile.SelectNodes("//message")) 
				{
					//get the messageID
					XmlAttribute messageIDAttribute = messageNode.Attributes["xmi:id"];
				 	string messageID = messageIDAttribute != null ? messageIDAttribute.Value: string.Empty;
					//get the source lifelineID
					string sourceID = getLifeLineID(messageNode,true,nsMgr);
				 	//get the target lifelineID
				 	string targetID = getLifeLineID(messageNode,false,nsMgr);
				 	//get the name of the message
				 	XmlAttribute nameAttribute = messageNode.Attributes["name"];
				 	string messageName = nameAttribute != null ? nameAttribute.Value: string.Empty;
				 	//get the synchronous/asynchronous attribute
				 	XmlAttribute messageSortAttribute = messageNode.Attributes["messageSort"];
				 	bool asynchronousMessage = messageSortAttribute != null && "asynchSignal".Equals(messageSortAttribute.Value, StringComparison.InvariantCulture);
				 	//create message
				 	if (! string.IsNullOrEmpty(messageID)
				 		&& ! string.IsNullOrEmpty(sourceID)
				 	    && ! string.IsNullOrEmpty(targetID))
				 	{
				 		var mdMessage = new MDMessage(messageID,sourceID,targetID,messageName,asynchronousMessage);
				 		foundMessages.Add(mdMessage);
				 	}
				}
			}
			//set the list to the found messages
			_allMessages = foundMessages;
		}
		private string getLifeLineID(XmlNode messageNode, bool source,XmlNamespaceManager nsMgr)
		{
			//first get the occurenceID
			XmlAttribute occurenceIDAttribute = source ? messageNode.Attributes["sendEvent"] : messageNode.Attributes["receiveEvent"];
			if (occurenceIDAttribute != null)
			{
				string occurenceID = occurenceIDAttribute.Value;
				if (! string.IsNullOrEmpty(occurenceID))
				{
					//get the messageOccurenceNode
					XmlNode occurenceNode = messageNode.SelectSingleNode("..//fragment[@xmi:id='"+occurenceID+"']",nsMgr);
					if (occurenceNode != null)
					{
						//get the covered node
						XmlNode coveredNode = occurenceNode.SelectSingleNode("covered");
						if (coveredNode != null)
						{
							XmlAttribute idRefAttribute = coveredNode.Attributes["xmi:idref"];
							return idRefAttribute != null ? idRefAttribute.Value : string.Empty;
						}
					}
				}
			}
			//if not found then return empty string
			return string.Empty;
		}
		void getAllLifeLines()
		{
			var foundLifeLines = new Dictionary<string, string>();
			//first find all the Lifeline nodes
			foreach (var sourceFile in this.sourceFiles.Values)
			{
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				foreach (XmlNode lifeLineNode in sourceFile.SelectNodes("//lifeline")) 
				{
					//get the ID of the lifeline
					XmlAttribute idAttribute = lifeLineNode.Attributes["xmi:id"];
					if (idAttribute != null)
					{
						string lifelineID = idAttribute.Value;
						//get the represents attribute
						XmlAttribute representsAttribute = lifeLineNode.Attributes["represents"];
						if (representsAttribute != null)
						{
							string ownedAttributeID = representsAttribute.Value;
							//get the ownedAttribute that represents this lifeline
							XmlNode ownedAttributeNode = sourceFile.SelectSingleNode("//ownedAttribute[@xmi:id='"+ownedAttributeID+"']",nsMgr);
							if (ownedAttributeNode != null)
							{
								//get the type attribute
								XmlAttribute typeAttribute = ownedAttributeNode.Attributes["type"];
								if (typeAttribute != null)
								{
									string typeID = typeAttribute.Value;
									//add the lifeline to the list
									if (! string.IsNullOrEmpty(lifelineID)
									    && ! string.IsNullOrEmpty(typeID))
									{
										foundLifeLines.Add(lifelineID,typeID);
									}
								}
							}
						}
					}
				}
			}
			_allLifeLines = foundLifeLines;
		}

		void getAllClasses()
		{
			var foundClasses = new Dictionary<string, string>();
			//first find all the class nodes
			foreach (var sourceFile in this.sourceFiles.Values)
			{
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				foreach (XmlNode classNode in sourceFile.SelectNodes("//packagedElement [@xmi:type='uml:Class']")) 
				{
					//get the ID attribute
					XmlAttribute idAttribute = classNode.Attributes["xmi:id"];
					string classID = idAttribute!= null ? idAttribute.Value: string.Empty;
					//get the name ID
					XmlAttribute nameAttribute = classNode.Attributes["name"];
					string className = nameAttribute!= null ? nameAttribute.Value: string.Empty;
					if (idAttribute != null
					    && ! foundClasses.Keys.Contains(classID))
					{
						foundClasses.Add(classID,className);
					}
						
				} 
			}
			//set the found classes
			_allClasses = foundClasses;
		}

		/// <summary>
		/// returns a dictionary of all constraints found in the Magicdraw source files
		/// </summary>
		/// <returns></returns>
		private Dictionary<string,List<MDConstraint>> getAllConstraints()
		{
			Dictionary<string,List<MDConstraint>> foundConstraints = new Dictionary<string, List<MDConstraint>>();
			foreach (var sourceFile in this.sourceFiles.Values) 
			{
				foreach (XmlNode constraintNode in sourceFile.SelectNodes("//ownedRule"))
				{
					//get the parent node
					var parentNode = constraintNode.ParentNode;
					//loop the attributes to get he type and id of the parent 
					string parentType = string.Empty;
					string parentID = string.Empty;
					foreach (XmlAttribute parentAttribute in parentNode.Attributes) 
					{
						switch (parentAttribute.Name) 
						{
							case "xmi:type":
								parentType = parentAttribute.Value;
								break;
							case "xmi:id":
								parentID = parentAttribute.Value;
								break;
						}
					}
					//only interested in constraints on classes
					if (parentType == "uml:Class"
					    && !string.IsNullOrEmpty(parentID))
					{
						List<MDConstraint> constraints;
						//check if the dictioary already contains our parent element
						if (foundConstraints.ContainsKey(parentID))
					    {
							constraints = foundConstraints[parentID];
					    }
						else
						{
							//not found, so create new one
							constraints = new List<MDConstraint>();
							foundConstraints.Add(parentID,constraints);
						}
						//create the MDConstraint
						
						try
						{
							string name = constraintNode.Attributes["name"].Value;
							string body = string.Empty;
							XmlNode bodyNode = constraintNode.SelectSingleNode("./specification/body");
							if (bodyNode != null) body = bodyNode.InnerText;
							string language = string.Empty;
							XmlNode languageNode = constraintNode.SelectSingleNode("./specification/language");
							if (languageNode != null) language = languageNode.InnerText;
							//check if everything is filled in
							if (! string.IsNullOrEmpty(name)
							    && ! string.IsNullOrEmpty(body)
							    && ! string.IsNullOrEmpty(language))
							{
								//create new MDConstraint and add it to the list
								constraints.Add(new MDConstraint(name, body, language));
							}
						}
						catch(NullReferenceException)
						{
							//do nothing, constraints without name cannor be created
						}
					}
				} 
			}
			return foundConstraints;
		}

		void getAllDiagrams()
		{
			var foundDiagrams = new Dictionary<string, MDDiagram>();
			//first find all the diagram nodes
			foreach (var sourceFile in this.sourceFiles.Values) 
			{
				foreach (XmlNode diagramNode in sourceFile.SelectNodes("//ownedDiagram"))
				{
					string diagramName = diagramNode.Attributes["name"].Value;
					//get the ID of the diagram. this is a combination of the ID of the owner + the name of the diagram
					string diagramID = diagramNode.Attributes["ownerOfDiagram"].Value +""+ diagramName ;
					//get the streamcontentID like <binaryObject streamContentID=BINARY-f9279de7-2e1e-4644-98ca-e1e496b72a22 
					// because that is the file we need to read and use to figure out the diagramObjects
					XmlNode binaryObjectNode = diagramNode.SelectSingleNode(".//binaryObject");
					if (binaryObjectNode != null)
					{
						MDDiagram currentDiagram = null;
						try
						{
							string diagramContentFileName = binaryObjectNode.Attributes["streamContentID"].Value;
							
							//get the file with the given name
							//get the directory of the source file
							string sourceDirectory = Path.GetDirectoryName(sourceFile.BaseURI.Substring(8));
							string diagramFileName = Path.Combine(sourceDirectory,diagramContentFileName);
							if (File.Exists(diagramFileName))
							{
								//all this workaround is needed because xmi is not defined as prefix in the binary files of MD
								var xmlDiagram  =new XmlDocument();
								XmlReaderSettings settings = new XmlReaderSettings { NameTable = new NameTable() };
								XmlNamespaceManager xmlns = new XmlNamespaceManager(settings.NameTable);
								xmlns.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
								XmlParserContext context = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
								XmlReader reader = XmlReader.Create(diagramFileName, settings, context);
								xmlDiagram.Load(reader);
								
								//xmlDiagram.Load(diagramFileName);
								foreach (XmlNode diagramObjectNode in xmlDiagram.SelectNodes(".//mdElement")) 
								{
									//get the elementID
									string elementID = getElementID(diagramObjectNode);
									//get the umlType of the elementNode
									XmlAttribute umlTypeAttribute = diagramObjectNode.Attributes["elementClass"];
									string umlType = umlTypeAttribute != null ? umlTypeAttribute.Value : string.Empty;
									if (!string.IsNullOrEmpty(elementID)
									   || umlType == "Split")
									{
										//get the geometry
										var geometryNode = diagramObjectNode.SelectSingleNode(".//geometry");
										if (geometryNode != null
										    && ! string.IsNullOrEmpty(geometryNode.InnerText))
										{
											if (currentDiagram ==null) currentDiagram = new MDDiagram(diagramName);
											var diagramObject = new MDDiagramObject(elementID,geometryNode.InnerText,umlType);
											currentDiagram.addDiagramObject(diagramObject);
											if (umlType == "Split")
											{
												XmlNode fragmentNode = diagramObjectNode.ParentNode.ParentNode;
												if (fragmentNode.Name == "mdElement")
												{
													//get the id of the fragment
													string fragmentID = getElementID(fragmentNode);
													if (! string.IsNullOrEmpty(fragmentID))
													{
														//find the corresponding diagramObject from the current diagram
														var fragmentDiagramObject = currentDiagram.diagramObjects.FirstOrDefault (x => x.mdID.Equals(fragmentID,StringComparison.CurrentCultureIgnoreCase));
														if (fragmentDiagramObject != null)
														{
															fragmentDiagramObject.ownedSplits.Add(diagramObject);
														}
													}
												}
											}
										}
									}
								}
								//add the diagram to the list
								if (currentDiagram != null 
								    && ! foundDiagrams.ContainsKey(diagramID))
								{
									foundDiagrams.Add(diagramID,currentDiagram);
								}
							}
						}
						catch(NullReferenceException)
						{
							//do nothing, we can't do anything with bynary object nodes withotu a streamContentID
						}
					}
				}
			}
			_allDiagrams = foundDiagrams;
		}
		
		string getElementID(XmlNode diagramObjectNode)
		{
			string elementID = string.Empty;
		
			//get the elementID
			var elementIDNode = diagramObjectNode.SelectSingleNode(".//elementID");
			if (elementIDNode != null)
			{
				//first theck the href attribute
				XmlAttribute hrefAttribute = elementIDNode.Attributes["href"];
				
				if (hrefAttribute != null)
				{
					string fullHrefString = elementIDNode.Attributes["href"].Value;
					int seperatorIndex = fullHrefString.IndexOf('#');
					if (seperatorIndex >= 0 )
					{
						elementID =  fullHrefString.Substring(seperatorIndex +1);
					}
				}
				else
				{
					//check the "xmi:idref attribute
					XmlAttribute idRefAttribute = elementIDNode.Attributes["xmi:idref"];
					if (idRefAttribute != null) elementID = idRefAttribute.Value;
				}
			}
			return elementID;
		}
		
		
		
		void getAllObjects()
		{
			var foundObjects = new Dictionary<string, string>();
			//first find all the object nodes
			
			foreach (var sourceFile in this.sourceFiles.Values) 
			{
				string objectId = "", inState = "", objectState = "";
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				//[@xmi:type='uml:CentralBufferNode']"
				
				foreach (XmlNode objectNode in sourceFile.SelectNodes("//node[@xmi:type='uml:CentralBufferNode']", nsMgr))
				{
					try
					{
						var id = objectNode.Attributes["xmi:id"].Value;
						if (id != null)
						{
							objectId = id; //md_guid
							EAOutputLogger.log(this.model,this.outputName
					                   	,string.Format("{0} Getting objectNode with ObjectID: '{1}'"
	                                  	,DateTime.Now.ToLongTimeString()
	                                  	,objectId)
	                                	
	                   		,0
	                  		,LogTypeEnum.log);
						}
						
						XmlNode inStateNode = objectNode.SelectSingleNode(".//inState");
						if(inStateNode != null)
						{
							var idref = inStateNode.Attributes["xmi:idref"].Value;
							if (idref != null)
							{
								inState = idref;
								EAOutputLogger.log(this.model,this.outputName
					                   	,string.Format("{0} Getting inState value '{1}'"
	                                  	,DateTime.Now.ToLongTimeString()
	                                  	,inState)
	                                	
	                   		,0
	                  		,LogTypeEnum.log);
							}			
						}
					
						XmlNode stateNode = sourceFile.SelectSingleNode("//subvertex[@xmi:type='uml:State' and @xmi:id='"+ inState +"']", nsMgr);
						if(stateNode != null)
						{
							var name = stateNode.Attributes["name"].Value;
							if (name != null)
							{
								objectState = name;
								EAOutputLogger.log(this.model,this.outputName
					                   	,string.Format("{0} Getting object state '{1}'"
	                                  	,DateTime.Now.ToLongTimeString()
	                                  	,objectState)
	                                	
	                   			,0
	                  			,LogTypeEnum.log);
							}	
						}
					}
					catch (NullReferenceException)
					{
			
						
					}	
					if (!string.IsNullOrEmpty(objectId) 
					    && !string.IsNullOrEmpty(inState) 
					    && !string.IsNullOrEmpty(objectState)
					    && !foundObjects.ContainsKey(objectId))
					{
						foundObjects.Add(objectId,objectState);
					}
				
				}
				
			}
			_allObjects = foundObjects;
		}
		
		void getAllDependencies()
		{
			var foundDependencies = new Dictionary<string, string>();
			foreach (var sourceFile in this.sourceFiles.Values) 
			{
				
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				
				foreach (XmlNode mapsToNode in sourceFile.SelectNodes("//*[local-name() = 'mapsTo']",nsMgr)) 
				{
					
					XmlAttribute dependencyAttribute = mapsToNode.Attributes["base_Dependency"];
					
					if (dependencyAttribute != null)
					{
						string dependencyID = dependencyAttribute.Value;
						
						
						foreach (XmlNode dependencyNode in sourceFile.SelectNodes("//packagedElement[@xmi:id='"+dependencyID+"']",nsMgr))	
						{
							string source = "", target = "";
							
							//select client node, attribute idref
							XmlNode clientNode = dependencyNode.SelectSingleNode("./client");
							if (clientNode != null)
							{
								XmlAttribute clientAttribute = clientNode.Attributes["xmi:idref"];
								source = clientAttribute != null? clientAttribute.Value: string.Empty;
							}
							
							// select supplier node, attribute href, after #
							XmlNode supplierNode = dependencyNode.SelectSingleNode("./supplier");
							if (supplierNode != null)
							{
								XmlAttribute supplierAttribute = supplierNode.Attributes["href"];
								string fullHrefValue = supplierAttribute != null ? supplierAttribute.Value : string.Empty;
								//get the part after the # sign
								var splittedHref = fullHrefValue.Split('#');
								if (splittedHref.Count() == 2)
								{
									target = splittedHref[1];
								}
							}
							
							if (!string.IsNullOrEmpty(target) & !string.IsNullOrEmpty(source))
							{
									try
									{
										foundDependencies.Add(source,target);
									}
									catch(Exception e)
									{
										
									}
							}
						
						}
							
					}
					
					
				}
			}
			
			
			_allDependencies = foundDependencies;
		}
		
		
		void getAllMatrixes()
		{
			var foundMatrixes = new Dictionary<string, string>();
			
			foreach (var sourceFile in this.sourceFiles.Values) 
			{
				
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				
				foreach (XmlNode MatrixNode in sourceFile.SelectNodes("//*[local-name() = 'DependencyMatrix']",nsMgr)) 
				{
					XmlAttribute rowCustomOrderAttribute = MatrixNode.Attributes["rowCustomOrder"];
					
					if (rowCustomOrderAttribute != null)
					{
						string rowCustomOrder = rowCustomOrderAttribute.Value;
						Debug.WriteLine(rowCustomOrder);
					}
				}
			}
			_allMatrixes = foundMatrixes;
		}
		
		void getAllAttributes()
		{
			var foundAttributes = new List<MDAttribute>();
			
			foreach (var sourceFile in this.sourceFiles.Values)
			{
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				
				
				//node -> ownedAttribute
				//where attribute association not present
				
				foreach (XmlNode attributeNode in sourceFile.SelectNodes(" ", nsMgr))
				{
					
				}
			}
		}
			
			
		
		void getAllPartitions()
		{
			var foundPartitions = new Dictionary<string, string>();
			
			//first find all the object nodes
			foreach (var sourceFile in this.sourceFiles.Values) 
			{
				string partitionID = "", representsID = "";
				XmlNamespaceManager nsMgr = new XmlNamespaceManager(sourceFile.NameTable);
				nsMgr.AddNamespace("xmi", "http://www.omg.org/spec/XMI/20131001");
				nsMgr.AddNamespace("uml", "http://www.omg.org/spec/UML/20131001");
				
				foreach (XmlNode partitionNode in sourceFile.SelectNodes("//group[@xmi:type='uml:ActivityPartition']", nsMgr))
				{
					//Look for the <group> node based on the md_guid, md_guid = xmi:id in the group node
					try
					{
						//Get the partition id
						var partition = partitionNode.Attributes["xmi:id"].Value;
						if (partition != null)
						{
							partitionID = partition;
							EAOutputLogger.log(this.model,this.outputName
					                   	,string.Format("{0} Getting partitionID: '{1}'"
	                                  	,DateTime.Now.ToLongTimeString()
	                                  	,partitionID)
	                                	
	                   		,0
	                  		,LogTypeEnum.log);
						}
						
						//Get the represents id
						var represents = partitionNode.Attributes["represents"].Value;
						if (represents != null)
						{
							representsID = represents;
							EAOutputLogger.log(this.model,this.outputName
					                   	,string.Format("{0} Getting representsID: '{1}'"
	                                  	,DateTime.Now.ToLongTimeString()
	                                  	,representsID)
	                                	
	                   		,0
	                  		,LogTypeEnum.log);
						}
					}
					catch (NullReferenceException)
					{
			
						
					}	
					if(!string.IsNullOrEmpty(partitionID) 
					   && !string.IsNullOrEmpty(representsID)
					   && !foundPartitions.ContainsKey(partitionID))
					{
						foundPartitions.Add(partitionID, representsID);
					}
				}
				
			}
			_allPartitions = foundPartitions;
		}
		
		public string getDiagramOnwerID(string diagramID)
		{
			return diagramID.Substring(0,diagramID.IndexOf(""));
		}
		
		private void getAllAssociationTables()
		{
			var foundTables = new Dictionary<string,string>();
			//find the comment nodes containing the association tables.
			//these are on the diagrams in MagicDraw.
			foreach (var sourceFile in this.sourceFiles.Values) 
			{
				foreach (XmlNode constraintNode in sourceFile.SelectNodes("//ownedDiagram/ownedComment"))
				{
					
					//get the body attribute and check if it starts with "'&lt;" (html content)
					try
					{
						XmlAttribute bodyAttribute = constraintNode.Attributes["body"];
						if (bodyAttribute.Value.StartsWith("<html>"))
					    {
					    	//we have html comments
					    	//get the parentPackage node
					    	XmlNode packageNode = constraintNode.ParentNode.ParentNode.ParentNode.ParentNode;
					    	if (packageNode.Name == "packagedElement" && packageNode.Attributes["xmi:type"].Value == "uml:Package" )
					    	{
					    		//OK we have the package node. Now check if it contains a class with the same name as the diagram
						    	//check if there is a class in the owning package with the same name
						    	foreach (XmlNode ownedElementNode in packageNode.ChildNodes) 
						    	{
						    		if (ownedElementNode.Name == "packagedElement" 
						    		    && ownedElementNode.Attributes["xmi:type"].Value == "uml:Class" //is of type class
						    		    && ownedElementNode.Attributes["name"].Value.Equals(constraintNode.ParentNode.Attributes["name"].Value,StringComparison.InvariantCultureIgnoreCase)) //name corresponds to the diagram
						    		{
						    			string tableID = ownedElementNode.Attributes["xmi:id"].Value;
						    			if (! foundTables.ContainsKey(tableID))
						    			{
						    				//actually add the association table to the list
						    				foundTables.Add(tableID,bodyAttribute.Value);
						    			}
						    		}
						    	}
					    	}
					    }
					}
					catch(NullReferenceException)
					{
						//do nothing, constraints without name cannor be created
					}				
				}
			}
			//save the association tables
			_allLinkedAssociationTables = foundTables; 
			
		}
	}
}
