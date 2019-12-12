import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class TvShow extends Component {
    static displayName = TvShow.name;

    constructor(props) {
        super(props);
        this.state = { shows: [], loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderShows(shows) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                <tr>
                    <th>Name</th>
                    <th>Next episode</th>
                    <th>Number of seasons</th>
                </tr>
                </thead>
                <tbody>
                {shows.map(show =>
                    <tr key={show.id}>
                        <td>{show.name}</td>
                        <td>{show.nextEpisode}</td>
                        <td>{show.numberOfSeasons}</td>
                    </tr>
                )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p>
                  <em>Loading...</em>
              </p>
            : TvShow.renderShows(this.state.tvShows);

        return (
            <div>
                <h1 id="tabelLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const token = await authService.getAccessToken();
        const response = await fetch('tvshow',
            {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });
        const tvShows = await response.json();
        this.setState({ tvShows: tvShows, loading: false });
    }
}