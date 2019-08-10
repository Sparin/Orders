import React from 'react';
import { Table, Divider, Tag, Form, Button, Tooltip, Collapse, Dropdown, Menu, Icon, Pagination } from 'antd';
import moment from 'moment';
import './Orders.css';
import SearchFilters from '../../components/SearchFilters/SearchFilters';
import Search from 'antd/lib/input/Search';
import { searchOrders } from '../../api/order/methods';
import { SearchResponse, Order, SearchOptions } from '../../api/order/models';
import { defaultPage } from '../../api/makeHttpRequest';

const { Column, ColumnGroup } = Table;
const { Panel } = Collapse;



export default class Orders extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = {
            orders: [],
            isLoading: false,
            currentPage: 1,
            limit: 50,
            totalPages: 0,
            totalItems: 0,
            searchOptions: {}
        };

        this.applySearchFilters = this.applySearchFilters.bind(this);
    }

    componentDidMount() {
        this.updateOrders(this.state.currentPage, this.state.limit);
    }

    componentDidUpdate(prevProps: any, prevState: any) {
        if (prevState.searchOptions !== this.state.searchOptions)
            this.updateOrders(1, this.state.limit)
    }

    async updateOrders(page: number, limit: number) {
        this.setState({ isLoading: true })
        const results = await searchOrders(page - 1, limit, this.state.searchOptions) as SearchResponse<Order>
        this.setState({
            currentPage: results.currentPage + 1,
            totalPages: results.totalPages,
            totalItems: results.totalItems,
            orders: results.items,
            isLoading: false
        });
    }

    applySearchFilters(options: SearchOptions) {
        this.setState({ searchOptions: options });

    }

    render() {
        const menu = (
            <Menu >
                <Menu.Item key="1">
                    <Icon type="edit" />
                    Edit
              </Menu.Item>
                <Menu.Item key="2">
                    <Icon type="delete" />
                    Delete
              </Menu.Item>
            </Menu>
        );

        return (
            <div className="orders-layout-content">

                <div className="table-control-bar">
                    <Button type="primary" icon="form">Create</Button>
                </div>

                <div className="table-filters-bar">
                    <Collapse bordered={false}>
                        <Panel header="Filters" key="1">
                            <SearchFilters onApply={this.applySearchFilters} />
                        </Panel>
                    </Collapse>
                </div>

                <Table dataSource={this.state.orders}
                    style={{ marginTop: "8px" }}
                    rowKey="id"
                    pagination={{
                        onChange: (page, size) =>
                            this.updateOrders(page, size as number)
                        ,
                        showTotal: (total, range) => `Total: ${total}`,
                        current: this.state.currentPage,
                        total: this.state.totalItems,
                        pageSize: this.state.limit
                    }}>
                    <Column title="Id" dataIndex="id" />
                    <Column title="First Name" dataIndex="firstName" />
                    <Column title="Middle Name" dataIndex="middleName" />
                    <Column title="Last Name" dataIndex="lastName" />
                    <Column title="Address" dataIndex="address" />
                    <Column title="Fruit" dataIndex="fruit" />
                    <Column title="Amount" dataIndex="amount" />
                    <Column title="ETA" dataIndex="eta"
                        render={(text, record) => {
                            console.log(text);
                            console.log(record);
                            const date = new Date(text)
                            return (
                                <Tooltip title={moment(date).format('DD.MM.YYYY HH:mm:ss')}>
                                    <span>{moment(date).fromNow()}</span>
                                </Tooltip>)
                        }} />
                    <Column
                        title=""

                        render={(text, record) => (
                            <Dropdown overlay={menu}>
                                <Icon type="menu" />
                            </Dropdown>
                        )}
                    />
                </Table>
            </div >);
    }
}