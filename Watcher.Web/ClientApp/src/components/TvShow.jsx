import React, { useState, useEffect } from 'react';
import Loading from './shared/Loading';

function renderShows(shows) {
    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
            <tr>
                <th>Name</th>
                <th>Description</th>
            </tr>
            </thead>
            <tbody>
            {shows.map(show =>
                    <tr key={show.id}>
                        <td>{show.name}</td>
                        <td>{show.description}</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}

export function TvShow() {
    const [shows, setShows] = useState(null);

    async function getData() {
        const response = await fetch('tvshow');

        const shows = await response.json();
        setShows(shows);
    }

    useEffect(() => {
        getData();
    },[]);

    const contents = shows == null ? <Loading /> : renderShows(shows);

    return (
        <div>
            <h1>Tv show overview</h1>
            {contents}
        </div>
    );
}