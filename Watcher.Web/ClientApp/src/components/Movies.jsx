import React, { useState, useEffect } from 'react';
import Loading from './shared/Loading';

function renderMovies(movies) {
    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Release date</th>
                </tr>
            </thead>
            <tbody>
                {movies.map(movie =>
                    <tr key={movie.id}>
                        <td>{movie.name}</td>
                        <td>{movie.description}</td>
                        <td>{movie.releaseDate}</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}

export function Movies() {
    const [movies, setMovies] = useState(null);

    async function getData() {
        const response = await fetch('movie');

        const movies = await response.json();
        setMovies(movies);
    }

    useEffect(() => {
        getData();
    }, []);

    const contents = movies == null ? <Loading /> : renderMovies(movies);

    return (
        <div>
            <h1>Movie overview</h1>
            {contents}
        </div>
    );
}