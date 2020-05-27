# CQRS-Template-Generator
Gerador de código para projetos CQRS com intuito de agilizar a criação dos CRUDS da entidade informada

Utilizado junto com o template CQRS:
LINK

O gerador espera uma solution com os seguintes projetos na estrutura:

- Nome_da_Solution.Core
- Nome_da_Solution.Web

Os códigos serão gerados para os seguintes namespaces:

- Nome_da_Solution.Core
  - Application
    - Interfaces
    - Services
    - ViewModels
  - Domain
    - CommandHandlers
    - Commands
    - Entities
    - EventHandlers
    - Events
    - Interfaces
    - Validations
  - Infrastructure
    - Config
      - Maps
    - Repositories
