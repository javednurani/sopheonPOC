import React, { Component, ErrorInfo, ReactNode } from 'react';

interface Props {
  children: ReactNode;
}

interface State {
  hasError: boolean;
  errorMessage: string;
}

class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
    errorMessage: '',
  };

  public static getDerivedStateFromError(_: Error): State {
    // Update state so the next render will show the fallback UI.
    return {
      hasError: true,
      errorMessage: _.message || 'Component could not be loaded.',
    };
  }

  public componentDidCatch(error: Error, errorInfo: ErrorInfo): void {
    // eslint-disable-next-line no-console
    console.log('Uncaught error:', error, errorInfo); // TODO: Better error handling than console log
  }

  public render(): ReactNode {
    if (this.state.hasError) {
      return <h1> {this.state.errorMessage} </h1>;
    }

    return this.props.children;
  }
}

export { ErrorBoundary };
