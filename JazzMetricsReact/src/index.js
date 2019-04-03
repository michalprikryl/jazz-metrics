import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import { createStore, combineReducers } from "redux";
import { sessionReducer, sessionService } from "redux-react-session";

import "./index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";

const reducers = {
  session: sessionReducer
};
const reducer = combineReducers(reducers);

const store = createStore(reducer);

sessionService.initSessionService(store, { driver: "LOCALSTORAGE" });

ReactDOM.render(
  <Provider store={store}>
    <BrowserRouter>
        <App />
    </BrowserRouter>
  </Provider>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: http://bit.ly/CRA-PWA
serviceWorker.unregister();
