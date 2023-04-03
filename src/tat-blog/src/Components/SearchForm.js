import { useState } from "react";
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";

const SearchForm = () => {
    const [Keyword, setKeyword] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        window.location = `/blog?k=${Keyword}}`;
};

return (
    <div className='mb-4'>
        <Form method='get' onSubmit={handleSubmit}>
            <Form.Group className='input-group mb-3'>
                <Form.Control 
                type ='text'
                name='k'
                value={Keyword}
                onChange={(e) => setKeyword(e.target.value)}
                aria-label='Enter Keyword'
                aria-describedby='btnSearchPost'
                placeholder='Enter Keyword' />
            <Button
                id ='btnSearchPost'
                variant='outline-secondary'
                type='submit'>
                    <FontAwesomeIcon icon={faSearch} />
            </Button>
            </Form.Group>
        </Form>
    </div>
    );
}

export default SearchForm;