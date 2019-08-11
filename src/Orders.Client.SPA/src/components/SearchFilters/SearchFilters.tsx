import React from 'react';
import { Form, DatePicker, Button, Slider, Select, Checkbox } from 'antd';
import { Fruit, SearchOptions } from '../../api/order/models';

const { RangePicker } = DatePicker;
const { Option } = Select;

export default class SearchFilters extends React.Component<any, any> {

    fruits = Object.keys(Fruit).filter((value, index, self) => self.indexOf(value) === index);

    constructor(props: any) {
        super(props);
        const fruits = Object.keys(Fruit).filter((value, index, self) => self.indexOf(value) === index);
        this.state = { orderBy: "Id", fruits, minimumAmount: 5, maximumAmount: 10000 } as SearchOptions;
    }

    onSubmit = (event: any) => {
        if (event)
            event.preventDefault();
        this.props.onApply(this.state);
    }

    render() {


        return (
            <Form layout="vertical" onSubmit={this.onSubmit}>
                <Form.Item label="Estimated time of arrival">
                    <RangePicker
                        showTime
                        format="YYYY-MM-DD HH:mm:ss"
                        onChange={(dates, dateS) => this.setState({ fromETA: dateS[0], untilETA: dateS[1] })} />
                </Form.Item>

                <Form.Item label="Amount">
                    <Slider
                        range
                        value={[this.state.minimumAmount, this.state.maximumAmount]}
                        max={10000}
                        onChange={(values: any) => this.setState({ minimumAmount: values[0], maximumAmount: values[1] })} />
                </Form.Item>

                <Form.Item label="Fruits">
                    <Select
                        mode="multiple"
                        style={{ width: '100%' }}
                        placeholder="Please select fruits for searching"
                        value={this.state.fruits}
                        onChange={(value: string[]) => this.setState({ fruits: value })}
                    >
                        {this.fruits.map(x => (<Option value={x} key={x}>{x}</Option>))}

                    </Select>
                </Form.Item>

                <Form.Item label="Order by">
                    <Select
                        style={{ width: 200 }}
                        placeholder="Select a field"
                        optionFilterProp="children"
                        defaultValue={this.state.orderBy}
                        onChange={(e: string) => this.setState({ orderBy: e })}
                    >
                        <Option value="Id">Id</Option>
                        <Option value="LastName">Last Name</Option>
                        <Option value="Amount">Amount</Option>
                        <Option value="ETA">ETA</Option>
                    </Select>
                    <Checkbox style={{ marginLeft: "16px" }}
                        onChange={(e) => this.setState({ descending: e.target.checked })}>Descending</Checkbox>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit">Apply</Button>
                </Form.Item>
            </Form>
        )
    }
}