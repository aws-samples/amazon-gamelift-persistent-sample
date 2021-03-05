
# Persistent Game Server with Amazon GameLift

This sample includes how to implement persistent game server based on Amazon GameLift.

This sample game includes 2 World Server Fleets for Persistent Game service.

* First Game World

![Game Client1](./demo-1.png)

* Second Game World (Boss Included!)

![Game Client2](./demo-2.png)

  - Provides GameLift sample for Unity
  - Implement scalable architecture for MMO like persistent game servers.
  - Include Latest best practices for GameLift Architecture.

# GameLift Architecture Features

![Architecture](./architecture.png)

  - Design multiple queues and fleets for each world servers. (Scalable!)
  - Implement poll-based patterns for matching scalable users.

# Workshop
 - EN : 
 - KO : 

### Todos
 - Add other modules (Login, Game Result Handling)
 - Add large match rules
 - Design Workshop

### Installation

 - Clone this Repository 
 To start this repository, you can Run this command for this repository to clone.
 Or you are able to follow Instructions from our [Workshop Material]()

 - Install Unity MLAPI
 Unity MLAPI is an open source framework that simplifies building networked games in Unity.
 As this library joins the Unity ecosystem, this project includes MLAPI as its Networking Library. ([Official Blog](https://blogs.unity3d.com/2020/12/03/accelerating-unitys-new-gameobjects-multiplayer-networking-framework/?_ga=2.256478095.584731899.1613809160-1332364721.1600864695))
 In order to run this sample, you need to import MLAPI source code / DLL in this repository.

 You are able to follow one of following guide links to install MLAPI.     
 (1) [MLAPI Installation](https://github.com/Unity-Technologies/com.unity.multiplayer.mlapi/blob/master/docs/_docs/getting-started/installation.md)

 (2) [Workshop Material]()
 
 - Install GameLift SDK
 You should Install GameLift SDK from our [official homepage](https://aws.amazon.com/gamelift/getting-started/?nc1=h_ls).
 And It should be included under "Assets" folder.    

 This information is also included in our [Workshop Material]()

 - Install Through Unity Package
 You are able to run Unity Package to import tested source code. It will be easiest way to import working source codes.

## Security

See [CONTRIBUTING](CONTRIBUTING.md#security-issue-notifications) for more information.

## License

This library is licensed under the MIT-0 License. See the LICENSE file.

