import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Welcome to Watcher!</h1>
          <div>
                <p>
                    If you like watching TV-shows or movies this is the website for you! You can easily keep track of your favorite shows and movies without the need to check online for release dates.
                    After subscribing you will get automatically notified when it is available to watch!
                </p>
              <ul>
                  <li>Get an email when a new episode / movie is released!</li>
                  <li>Get an email when your favorite actor is playing in a new show / movie!</li>
              </ul>
            </div>
      </div>
    );
  }
}
