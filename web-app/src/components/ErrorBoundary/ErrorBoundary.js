import React from "react";

import { handleError } from "../../methods/errorHandler";

class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false };
  }

  async componentDidCatch(error, info) {
    this.setState({ hasError: true });
    await handleError(error, "ErrorBoundary", "Global");
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="App">
          <h1 className="text-white">Něco se pokazilo.</h1>
        </div>
      );
    }
    return this.props.children;
  }
}

export default ErrorBoundary;
