import React, { useState, useEffect } from 'react';
import Loading from './shared/Loading';

function renderPersons(persons) {
    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Name</th>
                </tr>
            </thead>
            <tbody>
                {persons.map(person =>
                    <tr key={person.id}>
                        <td>{person.name}</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}

export function PersonOverview() {
    const [persons, setPersons] = useState(null);

    async function getData() {
        const response = await fetch('person');

        const persons = await response.json();
        setPersons(persons);
    }

    useEffect(() => {
        getData();
    }, []);

    const contents = persons == null ? <Loading /> : renderPersons(persons);

    return (
        <div>
            <h1>Person overview</h1>
            {contents}
        </div>
    );
}