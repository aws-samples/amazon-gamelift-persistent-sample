
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

 - Clone this Repository include submodule(MLAPI)
 For this, you can Run this command for this repository to clone.
 > git clone --recurse-submodules https://github.com/<GITREPO.git>

 - Install GameLift SDK
 You should Install GameLift SDK from our [official homepage](https://aws.amazon.com/gamelift/getting-started/?nc1=h_ls).
 And It should be included under "Assets" folder.    

## Security

See [CONTRIBUTING](CONTRIBUTING.md#security-issue-notifications) for more information.

## License

This library is licensed under the MIT-0 License. See the LICENSE file.

