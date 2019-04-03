import React, { Component, Fragment } from "react";
import { withRouter } from "react-router-dom";

import { fetchBearerGet } from "../../methods/fetchWithTimeout";

class User extends Component {
  constructor(props) {
    super(props);
    this.state = {
      user: undefined,
      loading: true
    };
  }

  async componentDidMount() {
    const {
      match: { params }
    } = this.props;
    const response = await fetchBearerGet(`user/${params.userId}`);
    if (response.status >= 200 && response.status <= 304) {
      const data = await response.json();
      if (!data || data.length === 0) {
        this.setState({
          message: "Neznámý uživatel."
        });
      } else {
        this.setState({
          user: data,
          message: undefined
        });
      }
    } else {
      this.setState({
        message: "Při načítání nastala chyba."
      });
    }

    this.setState({ loading: false });
  }

  render() {
    const { user, loading, message } = this.state;
    if (loading) return <p className="text-white">Načítání...</p>;
    if (message) return <p className="text-white">{message}</p>;
    return (
      <div className="container">
        <div className="row">
          <div className="jumbotron col-12">
            {user ? (
              <Fragment>
                <h1 className="display-3">{user.firstName}</h1>
                <p className="lead">{user.lastName}</p>
                <hr className="my-4" />
                <p>Answers:</p>
                <p className="lead">nejlepší uživatel</p>
              </Fragment>
            ) : (
              <h1 className="display-3">Neznámý uživatel</h1>
            )}
          </div>
        </div>
      </div>
    );
  }
}

export default withRouter(User);
