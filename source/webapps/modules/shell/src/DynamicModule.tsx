import { Spinner } from '@fluentui/react';
import { ErrorBoundary } from '@sopheon/shared-ui';
import { ShellApiProps } from '@sopheon/shell-api';
import React, { ComponentType, lazy, LazyExoticComponent, Suspense, useEffect, useState } from 'react';

import { AppModule } from './settings/appModuleSettings';

//#region helpers

const REMOTE_MODULE_NAME = './App';

// This interface facilitates fetching of remote modules from the Window's Webpack shared scope
declare global {
  interface Window {
    [key: string]: unknown;
  }
}

// Keep track of dynamic loaded scripts.
const dynamicScripts: { [url: string]: Promise<void> } = {};

function createScriptElement(url: string): HTMLScriptElement {
  const element: HTMLScriptElement = document.createElement('script');
  element.src = url;
  element.type = 'text/javascript';
  element.async = true;
  return element;
}

// this method extends a script element with promises on 'onload' and 'onerror' events
function addElementLoadPromises(
  element: HTMLScriptElement,
  url: string,
  setReady: React.Dispatch<React.SetStateAction<boolean>>,
  setFailed: React.Dispatch<React.SetStateAction<boolean>>
): Promise<void> {
  return new Promise((resolve, reject) => {
    // when <script> tag is loaded, and script executed
    element.onload = () => {
      console.log('onload', url);
      setReady(true);
      resolve();
    };
    // if <script> tag load or script execution fails
    element.onerror = e => {
      console.log('onerror', url, e);
      setReady(false);
      setFailed(true);
      reject();
    };
  });
}

const loadScript = async (
  url: string,
  setReady: React.Dispatch<React.SetStateAction<boolean>>,
  setFailed: React.Dispatch<React.SetStateAction<boolean>>
): Promise<void> => {
  if (!url) {
    return;
  }
  console.log('loadScript', url);
  // set initial state
  setReady(false);
  setFailed(false);

  // Ensure the script is only loaded once.
  if (dynamicScripts[url]) {
    try {
      // await the Promise stored at dynamicScripts['/someRemoteLocation/remoteEntry.js']
      console.log('loadScript existing');
      await dynamicScripts[url];
      setReady(true);
    } catch (e) {
      console.log('loadScript catch', e);
      setReady(false);
      setFailed(true);
      delete dynamicScripts[url];
    }
    return;
  }

  // create HTMLElement (script tag) to be appended to DOM head
  const element: HTMLScriptElement = createScriptElement(url);

  // add promises for onload and onerror
  const scriptPromise: Promise<void> = addElementLoadPromises(element, url, setReady, setFailed);

  // store promise at dynamicScripts['/someRemoteLocation/remoteEntry.js']
  dynamicScripts[url] = scriptPromise;

  // append HTMLElement (script tag) to DOM head
  // this will eventually trigger either onload or onerror for script element
  document.head.appendChild(element);
};

//#endregion

//#region dynamic methods

// react hook method for side effects
const useRemoteEntryScript = (url: string) => {
  const [ready, setReady] = useState(false);
  const [failed, setFailed] = useState(false);
  useEffect(() => {
    loadScript(url, setReady, setFailed);
  }, [url]);
  return {
    ready,
    failed,
  };
};

const loadDynamicRemoteModule = async (scope: string, module: string): Promise<unknown> => {
  console.log('loadDynamicRemoteModule - start');
  // Initializes the shared scope. This fills it with known provided modules from this build and all remotes.
  // @ts-ignore
  await __webpack_init_sharing__('default');

  // Get the remote container.
  const container = window[scope];
  console.log(scope, container);
  // Initialize the container with the shared scope, it may provide shared modules.
  // @ts-ignore
  await container.init(__webpack_share_scopes__.default);

  // Load the remote module (./App) from the remote container.
  // @ts-ignore
  const factory = await container.get(module);
  console.log('loadDynamicRemoteModule - end');
  return factory();
};

//#endregion dynamic methods

// this class should probably not be used directly, consider using "DynamicModule" instead
const DynamicModuleInternal: React.FC<DynamicModuleProps> = ({ module, loadingMessage, shellApi }: DynamicModuleProps) => {
  console.log('DynamicModuleInternal', module.url);
  const { ready, failed } = useRemoteEntryScript(module.url);

  if (!module) {
    throw new Error('No module specified');
  }

  if (failed) {
    throw new Error(`Failed to load dynamic script: ${module.url}`);
  }

  if (!ready) {
    return <Spinner label={loadingMessage} />;
  }

  const Module: LazyExoticComponent<ComponentType<unknown & ShellApiProps>> = lazy(() => loadDynamicRemoteModule(module.scope, REMOTE_MODULE_NAME));

  return (
    <Suspense fallback={<Spinner label={loadingMessage} />}>
      <Module shellApi={shellApi} />
    </Suspense>
  );
};

export type DynamicModuleProps = {
  module: AppModule;
  loadingMessage: string;
} & ShellApiProps;

// the purpose of this class is simply to wrap "DynamicModuleInternal" in an error boundary
export const DynamicModule: React.FC<DynamicModuleProps> = ({ module, loadingMessage, shellApi }: DynamicModuleProps) => (
  <ErrorBoundary>
    <DynamicModuleInternal module={module} loadingMessage={loadingMessage} shellApi={shellApi} />
  </ErrorBoundary>
);
