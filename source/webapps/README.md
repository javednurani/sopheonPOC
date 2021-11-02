# `Accolade NextGen Accolade NextGen Architecture playground`

## IMPORTANT

This folder is the output of Sopheon's create-accolade-app SHELL template.
This is intended to be a full-fledged solution, consisting of a Shell, sub-App, Shell API, and Shared-UI package.

Info and instructions to use this template output can be found here:
https://pluto/display/PDP/Developing+Cloud+Native+Applications

## Dev/Test Environment

- [React](https://reactjs.org/) for building user interfaces
- [TypeScript](https://www.typescriptlang.org/) as programming language
- [Webpack](https://webpack.js.org/) as module bundler
- [Babel](https://babeljs.io/) as ECMAScript transpiler
- [SASS](https://sass-lang.com/) as CSS transpiler
- [JEST](https://jestjs.io/) + [Enzyme](https://github.com/enzymejs/enzyme) as testing framework

- [react-router-dom](https://github.com/ReactTraining/react-router) for declarative routing
- [bootstrap](https://github.com/twbs/bootstrap/) for styling and common components
- [react-bootstrap](https://github.com/react-bootstrap/react-bootstrap)for React components based on Bootstrap.
- [fontawesome-free](https://github.com/FortAwesome/Font-Awesome) for icons
- [moment](https://github.com/moment/moment) for formatting dates and times
- [msal](https://github.com/AzureAD/microsoft-authentication-library-for-js) for authenticating to Azure Active Directory and retrieving access tokens
- [microsoft-graph-client](https://github.com/microsoftgraph/msgraph-sdk-javascript) for making calls to Microsoft Graph

## Commands

- Start Development Server: `npm start`
- Creating a Production Build: `npm run build`
- Running all Unit Tests: `npm test`
- Running all Unit Tests /w code coverage: `npm run test:coverage`
- Continuous Unit Testing: `npm run test:watch`

## Setup

1. Install **[Node 12 or newer](https://nodejs.org)**. Need to run multiple versions of Node? Use [nvm](https://github.com/creationix/nvm) or [nvm-windows](https://github.com/coreybutler/nvm-windows).
2. Install **[Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git)**.
3. Install **[Visual Studio Code](https://code.visualstudio.com/)**.
4. Install recommended **Visual Studio Code extensions**. You will be prompted to install the extensions after opening starter kit in Visiual Studio Code.
5. Install Lerna globally if not already done: `npm install lerna -g`
6. Install Sopheon packages using `lerna exec --parallel --no-bail --scope @sopheon/* 'npm i && npm run build'`
7. Install Sopheon modules using `lerna exec --parallel --no-bail --scope *-module 'npm i && npm run build'`
8. **Install Node Packages**: `npm install`
9. Start dev environment: `npm start`

- project structure / framework features:
  - HTTP client (fetch or Axios)
  - Internationalization: react-intl
  - Redux
  - Dialogs
