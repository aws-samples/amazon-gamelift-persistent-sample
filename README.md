
# Persistent Game Server with Amazon GameLift

This sample includes how to implement persistent game server based on Amazon GameLift.

  - Provides GameLift sample for Unity
  - Implement scalable architecture for MMO like persistent game servers.
  - Include Latest best practices for GameLift Architecture.

# GameLift Architecture Features

  - Design multiple queues and fleets for each world servers. (Scalable!)
  - Implement poll-based patterns for matching scalable users.

### Todos

 - Add other modules (Login, Game Result Handling)
 - Add large match rules
 - Design Workshop

### Installation

 - Clone this Repository 
 For this, you can Run this command for this repository to clone.

 - Install Unity MLAPI
 Unity MLAPI is an open source framework that simplifies building networked games in Unity.
 As this library joins the Unity ecosystem, this project includes MLAPI as its Networking Library. ([Official Blog](https://blogs.unity3d.com/2020/12/03/accelerating-unitys-new-gameobjects-multiplayer-networking-framework/?_ga=2.256478095.584731899.1613809160-1332364721.1600864695))
 In order to run this sample, you need to import MLAPI source code / DLL in this repository.

 You are able to follow one of following guide links to install MLAPI.
 (1) [MLAPI Installation](https://github.com/Unity-Technologies/com.unity.multiplayer.mlapi/blob/master/docs/_docs/getting-started/installation.md)

 (2) [Sample Workshop Material](Workshop Link)

 - Install GameLift SDK
 You should Install GameLift SDK from our [official homepage](https://aws.amazon.com/gamelift/getting-started/?nc1=h_ls).
 And It should be included under "Assets" folder.    

 This information is also included in our [Workshop Material]()

## Security

See [CONTRIBUTING](CONTRIBUTING.md#security-issue-notifications) for more information.

## License

This library is licensed under the MIT-0 License. See the LICENSE file.

