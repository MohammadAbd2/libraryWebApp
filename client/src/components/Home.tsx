import {useState, useEffect} from "react";
import '../App.css'
import {dev_backend_url} from "../baseUrl.ts";



interface Author {
    id: number;
    name: string;
}



function Home() {
    const [authors, setAuthors] = useState<Author[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);


    useEffect(() => {
        fetch(`${dev_backend_url}/api/authors`) // assuming your backend endpoint is /authors
            .then((res) => {
                if (!res.ok) throw new Error("Failed to fetch authors");
                return res.json();
            })
            .then((data) => {
                setAuthors(data);
                setLoading(false);
            })
            .catch((err) => {
                setError(err.message);
                setLoading(false);
            });
    }, []);

    if (loading) return <div>Loading authors...</div>;
    if (error) return <div>Error: {error}</div>;

    return (
        <>
            <div className="overflow-x-auto rounded-box border border-base-content/5 bg-base-100">
                <table className="table">
                    <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                    </tr>
                    </thead>
                    <tbody>
                    {authors.map((author) => (
                        <tr key={author.id} className="hover:bg-base-300">
                            <td>{author.id}</td>
                            <td>{author.name}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </>
    );
}

export default Home;